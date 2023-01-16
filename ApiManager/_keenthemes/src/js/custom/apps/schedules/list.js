"use strict";

var SchedulesDatatable = function () {


    var initScheduleDatatable = () => {

        scheduleDatatable = $('#scheduleTable').DataTable({
            serverSide: true,
            processing: false,
            stateSave: true,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            ajax: {
                url: 'https://localhost:7173/graphql',
                type: 'POST',
                contentType: 'application/json',
                data: function (d) {
                    var gql = `
                    query schedulesByTask($options: DatatableOptionsInput!) {
                        schedulesByTask(options: $options) {
                            data {
                                id
                                startBoundery
                                endBoundery
                                type
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
                                taskId: taskEl.value
                            }
                        }
                    }

                    return JSON.stringify(query);
                },
                dataSrc: function (json) {

                    json.recordsTotal = json.data.schedulesByTask.recordsTotal;
                    json.recordsFiltered = json.data.schedulesByTask.recordsFiltered;
                    return json.data.schedulesByTask.data;
                }
            },
            columns: [
                {
                    data: 'id'
                },
                {
                    data: 'startBoundery'
                },
                {
                    data: 'endBoundery'
                },
                {
                    data: 'type'
                },
                {
                    data: 'enabled'
                },
                {
                    data: null
                }
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
							    <input class="form-check-input" type="checkbox" value="${row.id}" />
							</div>
                        `
                    }
                },
                {
                    targets: -2,
                    orderable: true,
                    render: function (data, type, row) {
                        var state = {
                            false: { 'title': 'Disabled', 'color': 'danger' },
                            true: { 'title': 'Enabled', 'color': 'success' },
                        };

                        return `
                        <span class="badge badge-${state[row.enabled].color}">${state[row.enabled].title}</span>
                    `;
                    }
                },
                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `
						<a href="#" class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">Actions
						<!--begin::Svg Icon | path: icons/duotune/arrows/arr072.svg-->
						<span class="svg-icon svg-icon-5 m-0">
							<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
								<path d="M11.4343 12.7344L7.25 8.55005C6.83579 8.13583 6.16421 8.13584 5.75 8.55005C5.33579 8.96426 5.33579 9.63583 5.75 10.05L11.2929 15.5929C11.6834 15.9835 12.3166 15.9835 12.7071 15.5929L18.25 10.05C18.6642 9.63584 18.6642 8.96426 18.25 8.55005C17.8358 8.13584 17.1642 8.13584 16.75 8.55005L12.5657 12.7344C12.2533 13.0468 11.7467 13.0468 11.4343 12.7344Z" fill="currentColor" />
							</svg>
						</span>
						<!--end::Svg Icon--></a>
						<!--begin::Menu-->
						<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
							<!--begin::Menu item-->
							<div class="menu-item px-3">
								<a href="/task/edit/${row.id}" class="menu-link px-3">Edit</a>
							</div>
							<!--end::Menu item-->
							<!--begin::Menu item-->
							<div class="menu-item px-3">
								<a href="#" class="menu-link px-3" data-schedules-table-filter="delete_row">Delete</a>
							</div>
							<!--end::Menu item-->
						</div>
						<!--end::Menu-->
                `;
                    }
                }

            ]

        });

        // Close button handler
        const closeButton = schedulesModalEl.querySelector('[data-kt-schedules-modal-action="close"]');
        closeButton.addEventListener('click', e => {
            e.preventDefault();

            schedulesModal.hide();
        });

        // Cancel button handler
        const cancelButton = schedulesModalEl.querySelector('[data-kt-schedules-modal-action="cancel"]');
        cancelButton.addEventListener('click', e => {
            e.preventDefault();

            schedulesModal.hide();
        });

    }


    return {
        init: function () {
            initDatatable();
        }
    }
}();

document.addEventListener("DOMContentLoaded", function () {
    SchedulesDatatable.init();
})
