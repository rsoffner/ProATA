(function ($) {

    'use strict';

    var signalrConnection;

    function initSignalR() {
        signalrConnection = new signalR.HubConnectionBuilder()
            .withUrl("/messageBrokerHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        signalrConnection.start().then(function () {
            console.log("SignalR Hub Connected");
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }

    function initDatatable() {
        $('#datatable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: endpointUrl,
                type: 'POST',
                contentType: 'application/json',
                data: function (d) {
                    var gql = `
                    query tasks($options: DatatableOptionsInput!) {
                        tasks(options: $options) {
                            data {
                                id
                                title
                                enabled
                            }
                            recordsTotal
                            recordsFiltered
                        }
                    }
                    `;

                    var query = {
                        operationName: null,
                        query: gql,
                        variables: {
                            options: {
                                paginate: {
                                    page: (d.start / d.length) + 1,
                                    limit: d.length
                                }
                            }
                        }
                    }

                    return JSON.stringify(query);
                },
                dataSrc: function (json) {
                    console.log(json);
                    json.recordsTotal = json.data.tasks.recordsTotal;
                    json.recordsFiltered = json.data.tasks.recordsFiltered;
                    return json.data.tasks.data;
                }
            },
            columns: [
                { data: 'title', title: 'Title' },
                {
                    orderable: true,
                    render: function (data, type, row, meta) {
                        var state = {
                            false: { 'title': 'Disabled', 'color': 'danger' },
                            true: { 'title': 'Enabled', 'color': 'success' },
                        };

                        return `
                        <span class="badge badge-soft-${state[row.enabled].color}">${state[row.enabled].title}</span>
                    `;
                    }
                },
                {
                    orderable: false,
                    data: 'queued',
                    title: '#Queued'
                },
                {
                    data: 'lastRunTime',
                    title: 'Last Run Time'
                },
                {
                    data: 'lastRunResult',
                    title: 'Last Run Result'
                },
                {
                    orderable: false,
                    render: function (data, type, row, meta) {
                        return `
                        <a class="btn btn-sm btn-outline-secondary btn-runnow" data-id="${row.id}">Run Now</a>
                    `;
                    }
                }
            ],
            order: [[1, 'asc']]
        })
            .on('draw', function () {
                $('.btn-runnow').click(function (event) {
                    const id = $(this).attr('data-id');

                    signalrConnection.invoke("CommandReceived", id, 1).catch(function (err) {
                        return console.error(err.toString());
                    });

                    event.preventDefault();
                });
        });
    }

    function init() {
        initSignalR();
        initDatatable();
    }

    init();

})(jQuery)
