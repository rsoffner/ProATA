"use strict";

var TasksDatatable = function () {

    const TaskCommand = {
        Create: 0,
        Run: 1,
        End: 2,
        Enable: 3,
        Disable: 4
    };

    var taskTable = document.getElementById('taskTable');
    var taskEl = document.getElementById('taskId');
    const schedulerEl = document.getElementById("SchedulerId");
    var taskDatatable;
    var toolbarBase;
    var toolbarSelected;
    var selectedCount;
    var taskId = '00000000-0000-0000-0000-000000000000';

    // Add Schedule Variables
    const schedulesModalEl = document.getElementById('modal_list_schedules');
    const schedulesModal = new bootstrap.Modal(schedulesModalEl);
    var scheduleTable = document.getElementById('scheduleTable');
    var scheduleDatatable;

    const addScheduleModalEl = document.getElementById('modal_add_schedule');
    const scheduleForm = document.getElementById('addScheduleForm');
    const addScheduleModal = new bootstrap.Modal(addScheduleModalEl);

    var initTaskDatatable = () => {

        taskDatatable = $('#taskTable').DataTable({
            serverSide: true,
            processing: true,
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
                {
                    data: 'id'
                },
                {
                    data: 'title'
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
                    targets: 1,
                    orderable: true,
                    render: function (data, type, row) {
                        return `
                            <a href="/task/edit/${row.id}">${row.title}</a>
                        `;
                    }
                },
                {
                    targets: 2,
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
								<a href="#" class="menu-link px-3" data-tasks-table-filter="delete_row">Delete</a>
							</div>
							<!--end::Menu item-->
							<!--begin::Menu item-->
							<div class="menu-item px-3">
								<a href="#" class="menu-link px-3" data-tasks-table-filter="show_schedules">Schedules</a>
							</div>
							<!--end::Menu item-->
						</div>
						<!--end::Menu-->
                `;
                    }
                }
            ],
            order: [[1, 'asc']]
        }).on('draw', function () {
            initToggleToolbar();
            toggleToolbars();
            handleDeleteRows();
            handleScheduleDialog();
            KTMenu.createInstances();
        });
    };

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
                                cronExpression
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
                                taskId: taskId
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
                    data: 'cronExpression'
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
                    targets: 1,
                    orderable: false,
                    render: function (data, type, row) {
                        var description = "";
                        $.ajax({
                            type: "POST",
                            url: "https://www.freeformatter.com/quartz-cron2text",
                            data: {
                                "expression": row.cronExpression
                            },
                            dataType: 'json',
                            success: function (response) {
                                if (response && response.description) {
                                   description = response.description;
                                } else {
                                    description = response.error;
                                }
                            }
                        });
                        return description;
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

        }).on('draw', function () {
            KTMenu.createInstances();
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


    // Delete task
    var handleDeleteRows = () => {
        // Select all delete buttons
        const deleteButtons = taskTable.querySelectorAll('[data-tasks-table-filter="delete_row"]');

        deleteButtons.forEach(d => {
            // Delete button on click
            d.addEventListener('click', function (e) {
                e.preventDefault();

                // Select parent row
                const parent = e.target.closest('tr');

                // Get task name
                const taskName = parent.querySelectorAll('td')[1].querySelectorAll('a')[0].innerText;

                // Get id
                const id = parent.querySelectorAll('td')[0].querySelectorAll('input')[0].value;

                // SweetAlert2 pop up --- official docs reference: https://sweetalert2.github.io/
                Swal.fire({
                    text: "Are you sure you want to delete " + taskName + "?",
                    icon: "warning",
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: "Yes, delete!",
                    cancelButtonText: "No, cancel",
                    customClass: {
                        confirmButton: "btn fw-bold btn-danger",
                        cancelButton: "btn fw-bold btn-active-light-primary"
                    }
                }).then(function (result) {
                    if (result.value) {
                        Swal.fire({
                            text: "You have deleted " + taskName + "!.",
                            icon: "success",
                            buttonsStyling: false,
                            confirmButtonText: "Ok, got it!",
                            customClass: {
                                confirmButton: "btn fw-bold btn-primary",
                            }
                        }).then(function () {
                            // Remove current row
                            axios.delete('/api/task/delete/' + id)
                                .then(function (response) {

                                })
                                .catch(function (error) {

                                });


                            taskDatatable.ajax.reload();
                        });
                    } else if (result.dismiss === 'cancel') {
                        Swal.fire({
                            text: customerName + " was not deleted.",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Ok, got it!",
                            customClass: {
                                confirmButton: "btn fw-bold btn-primary",
                            }
                        });
                    }
                });
            })
        });
    }

    // Filter Datatable
    var handleFilters = () => {
        // Select filter options
        const filterForm = document.querySelector('[data-kt-task-table-filter="form"]');
        const filterButton = filterForm.querySelector('[data-kt-task-table-filter="filter"]');
        const selectOptions = filterForm.querySelectorAll('select');

        // Filter datatable on submit
        filterButton.addEventListener('click', function () {
            var filterString = '';

            // Get filter values
            selectOptions.forEach((item, index) => {
                if (item.value && item.value !== '') {
                    if (index !== 0) {
                        filterString += ' ';
                    }

                    // Build filter value options
                    filterString += item.value;
                }
            });

            // Filter datatable --- official docs reference: https://datatables.net/reference/api/search()
            taskDatatable.ajax.reload();
        });
    }

    // Init toggle toolbar
    var initToggleToolbar = () => {
        // Toggle selected action toolbar
        // Select all checkboxes
        const checkboxes = taskTable.querySelectorAll('[type="checkbox"]');

        // Select elements
        toolbarBase = document.querySelector('[data-task-table-toolbar="base"]');
        toolbarSelected = document.querySelector('[data-task-table-toolbar="selected"]');
        selectedCount = document.querySelector('[data-task-table-select="selected_count"]');
        const deleteSelected = document.querySelector('[data-task-table-select="delete_selected"]');

        // Toggle delete selected toolbar
        checkboxes.forEach(c => {
            // Checkbox on click event
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });

        // Deleted selected rows
        deleteSelected.addEventListener('click', function () {
            // SweetAlert2 pop up --- official docs reference: https://sweetalert2.github.io/
            Swal.fire({
                text: "Are you sure you want to delete selected tasks?",
                icon: "warning",
                showCancelButton: true,
                buttonsStyling: false,
                confirmButtonText: "Yes, delete!",
                cancelButtonText: "No, cancel",
                customClass: {
                    confirmButton: "btn fw-bold btn-danger",
                    cancelButton: "btn fw-bold btn-active-light-primary"
                }
            }).then(function (result) {
                if (result.value) {
                    Swal.fire({
                        text: "You have deleted all selected tasks!.",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn fw-bold btn-primary",
                        }
                    }).then(function () {
                        // Remove all selected tasks
                        checkboxes.forEach(c => {
                            if (c.checked) {
                                datatable.row($(c.closest('tbody tr'))).remove().draw();
                            }
                        });

                        // Remove header checked box
                        const headerCheckbox = table.querySelectorAll('[type="checkbox"]')[0];
                        headerCheckbox.checked = false;
                    }).then(function () {
                        toggleToolbars(); // Detect checked checkboxes
                        initToggleToolbar(); // Re-init toolbar to recalculate checkboxes
                    });
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: "Selected schedules was not deleted.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn fw-bold btn-primary",
                        }
                    });
                }
            });
        });
    }

    // Toggle toolbars
    const toggleToolbars = () => {
        // Select refreshed checkbox DOM elements 
        const allCheckboxes = taskTable.querySelectorAll('tbody [type="checkbox"]');

        // Detect checkboxes state & count
        let checkedState = false;
        let count = 0;

        // Count checked boxes
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });

        // Toggle toolbars
        if (checkedState) {
            selectedCount.innerHTML = count;
            toolbarBase.classList.add('d-none');
            toolbarSelected.classList.remove('d-none');
        } else {
            toolbarBase.classList.remove('d-none');
            toolbarSelected.classList.add('d-none');
        }
    }

    var handleScheduleDialog = () => {

        const scheduleButtons = taskTable.querySelectorAll('[data-tasks-table-filter="show_schedules"]');

        scheduleButtons.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();

                // Select parent row
                const parent = e.target.closest('tr');

                // Get taskId
                const id = parent.querySelectorAll('td')[0].querySelectorAll('input')[0].value;

                
                taskId = id;
                // initScheduleDatatable();
                scheduleDatatable.ajax.reload();

                schedulesModal.show();
            });
        });

        // Close button handler
        const closeButton = schedulesModalEl.querySelector('[data-kt-schedules-modal-action="close"]');
        closeButton.addEventListener('click', e => {
            e.preventDefault();

            schedulesModal.hide();
        });
    }

    var handleAddScheduleDialog = () => {
        addScheduleModalEl.addEventListener('show.bs.modal', function (e) {

            var taskEl = e.currentTarget.querySelector('[name="Schedule.TaskId"]');
            taskEl.value = taskId;
        });
    }

    var initAddScheduleForm = () => {
        var validator = FormValidation.formValidation(
            scheduleForm,
            {
                fields: {
                    'StartBoundery': {
                        validators: {
                            notEmpty: {
                                message: 'Start is required'
                            }
                        }
                    },
                },

                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleInvalidClass: '',
                        eleValidClass: ''
                    })
                }
            }
        );

        /*
        const scheduleSection = addScheduleModalEl.querySelector('[data-schedule-form-section="schedule"]');
        const daysSection = addScheduleModalEl.querySelector('[data-schedule-form-section="days"]');
        const recurrenceSection = addScheduleModalEl.querySelector('[data-schedule-form-section="recurrence"]');
        const recurrenceType = addScheduleModalEl.querySelector('[data-schedule-form="recurrenceType"]');

        const typeSelector = addScheduleModalEl.querySelector('[name="Schedule.Type"]');
        typeSelector.addEventListener('change', e => {
            switch (e.target.value) {
                case '0': // Once
                    scheduleSection.classList.remove('d-none');
                    daysSection.classList.add('d-none');
                    recurrenceSection.classList.add('d-none');
                    break;
                case '1': // Daily
                    scheduleSection.classList.remove('d-none');
                    recurrenceSection.classList.remove('d-none');
                    recurrenceType.innerHTML = 'days';
                    daysSection.classList.add('d-none');
                    break;
                case '2': // Weekly
                    scheduleSection.classList.remove('d-none');
                    recurrenceSection.classList.remove('d-none');
                    recurrenceType.innerHTML = 'weeks on:';
                    daysSection.classList.remove('d-none');
                    break;
            }
        });
        */

        // Submit button handler
        const submitButton = addScheduleModalEl.querySelector('[data-kt-schedules-modal-action="submit"]');
        submitButton.addEventListener('click', e => {
            e.preventDefault();

            // Validate form before submit
            if (validator) {
                validator.validate().then(function (status) {
                    console.log('validated!');

                    if (status == 'Valid') {
                        // Show loading indication
                        submitButton.setAttribute('data-kt-indicator', 'on');

                        // Disable button to avoid multiple click 
                        submitButton.disabled = true;

                        // Get form data
                        const formData = new FormData(scheduleForm);

                        console.log([...formData]);

                        axios.post('/api/schedule/create', formData)
                            .then(function (response) {
                                // Remove loading indication
                                submitButton.removeAttribute('data-kt-indicator');

                                // Enable button
                                submitButton.disabled = false;

                                // Show popup confirmation 
                                Swal.fire({
                                    text: "Schedule has been successfully saved!",
                                    icon: "success",
                                    buttonsStyling: false,
                                    confirmButtonText: "Ok",
                                    customClass: {
                                        confirmButton: "btn btn-primary"
                                    }
                                }).then(function (result) {
                                    if (result.isConfirmed) {
                                        addScheduleModal.hide();
                                    }
                                });
                                console.log(repsonse);
                            })
                            .catch(function (error) {
                                console.log(error);
                            });
                    }

                });
            }
        });

        // Cancel button handler
        const cancelButton = addScheduleModalEl.querySelector('[data-kt-schedules-modal-action="cancel"]');
        cancelButton.addEventListener('click', e => {
            e.preventDefault();

            Swal.fire({
                text: "Are you sure you would like to cancel?",
                icon: "warning",
                showCancelButton: true,
                buttonsStyling: false,
                confirmButtonText: "Yes, cancel it!",
                cancelButtonText: "No, return",
                customClass: {
                    confirmButton: "btn btn-primary",
                    cancelButton: "btn btn-active-light"
                }
            }).then(function (result) {
                if (result.value) {
                    scheduleForm.reset(); // Reset form			
                    addScheduleModal.hide();
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: "Your form has not been cancelled!.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok!",
                        customClass: {
                            confirmButton: "btn btn-primary",
                        }
                    });
                }
            });
        });

        // Close button handler
        const closeButton = addScheduleModalEl.querySelector('[data-kt-schedules-modal-action="close"]');
        closeButton.addEventListener('click', e => {
            e.preventDefault();

            Swal.fire({
                text: "Are you sure you would like to cancel?",
                icon: "warning",
                showCancelButton: true,
                buttonsStyling: false,
                confirmButtonText: "Yes, cancel it!",
                cancelButtonText: "No, return",
                customClass: {
                    confirmButton: "btn btn-primary",
                    cancelButton: "btn btn-active-light"
                }
            }).then(function (result) {
                if (result.value) {
                    scheduleForm.reset(); // Reset form			
                    addScheduleModal.hide();
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: "Your form has not been cancelled!.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn btn-primary",
                        }
                    });
                }
            });
        });

    }

    return {
        init: function () {
            initTaskDatatable();
            initScheduleDatatable();
            initToggleToolbar();
            initAddScheduleForm();
            handleFilters();
            handleDeleteRows();
            handleScheduleDialog();
            handleAddScheduleDialog();
        }

    };

}();

document.addEventListener("DOMContentLoaded", function () {
    TasksDatatable.init();
})

