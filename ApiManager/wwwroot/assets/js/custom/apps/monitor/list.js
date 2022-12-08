"use strict";

var MonitorDatatable = function () {

    const TaskCommand = {
        Create:     0,
        Run:        1,
        End:        2,
        Enable:     3,
        Disable:    4
    };

    var datatable;
    var signalrConnection;

    const schedulerEl = document.getElementById("SchedulerId");

    var initDatatable = function () {
        

        datatable = $('#taskTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: 'https://localhost:7173/graphql',
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
                                },
                                schedulerId: schedulerEl.value
                            }
                        }
                    }

                    return JSON.stringify(query);
                },
                dataSrc: function (json) {
                    
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
                    // data: 'queued',
                    data: null,
                    title: '#Queued'
                },
                {
                    //data: 'lastRunTime',
                    data: null,
                    title: 'Last Run Time'
                },
                {
                    //data: 'lastRunResult',
                    data: null,
                    title: 'Last Run Result'
                },
                {
                    orderable: false,
                    render: function (data, type, row, meta) {
                        return `
                        <a class="btn btn-sm btn-outline-secondary" data-button="run" data-id="${row.id}">Run Now</a>
                    `;
                    }
                }
            ],
            order: [[1, 'asc']]
        }).on('draw', function () {
            const runButtons = document.querySelectorAll('[data-button="run"]');

            runButtons.forEach((item, index) => {
                item.addEventListener('click', function (event) {
                    signalrConnection.invoke("CommandReceived", schedulerEl.value, this.dataset.id, TaskCommand.Run).catch(function (err) {
                        return console.error(err.toString());
                    });
                    event.preventDefault();
               });
            });
        });
    };

    var initFilters = function () {
        schedulerEl.addEventListener('change', (event) => {

            datatable.ajax.reload();

        });
    }

    var initSignalR = function () {
        signalrConnection = new signalR.HubConnectionBuilder()
            .withUrl("/messagebroker")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        signalrConnection.start().then(function () {
            console.log("SignalR Hub Connected");
        }).catch(function (err) {
            return console.error(err.toString());
        });
    };

    return {
        init: function () {
            initDatatable();
            initFilters();
            initSignalR();
        }

    };

}();

document.addEventListener("DOMContentLoaded", function () {
    MonitorDatatable.init();
})

