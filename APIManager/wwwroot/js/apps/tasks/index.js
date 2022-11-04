$(document).ready(function () {
    $('#datatable').DataTable({
        serverSide: true,
        processing: true,
        select: "single",
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
            {
                className: 'details-control',
                orderable: false,
                data: null,
                defaultContent: ''
            },
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
                render: function (data, type, row, meta) {
                    return `
                        Edit
                    `;
                }
            }
        ],
        order: [[1, 'asc']]
    });

    $('#datatable tbody').on('click', 'td.details-control', function () {
        console.log('details clicked');
    });

});
