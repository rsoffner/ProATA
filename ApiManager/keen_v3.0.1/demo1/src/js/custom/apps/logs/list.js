"use strict";

var LogDatable = function () {

    // variables
    var datatable;
    var isPaused = false;
    var timer;

    // init
    var initDatatable = function () {
        const periodEl = document.getElementById("period");
        const userEl = document.getElementById("UserId");
        const taskEl = document.getElementById("TaskId");

        const playIcon = `
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-play-circle" viewBox="0 0 16 16">
              <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
              <path d="M6.271 5.055a.5.5 0 0 1 .52.038l3.5 2.5a.5.5 0 0 1 0 .814l-3.5 2.5A.5.5 0 0 1 6 10.5v-5a.5.5 0 0 1 .271-.445z"/>
            </svg>
        `;

        const pauseIcon = `
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pause-circle" viewBox="0 0 16 16">
              <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
              <path d="M5 6.25a1.25 1.25 0 1 1 2.5 0v3.5a1.25 1.25 0 1 1-2.5 0v-3.5zm3.5 0a1.25 1.25 0 1 1 2.5 0v3.5a1.25 1.25 0 1 1-2.5 0v-3.5z"/>
            </svg>            
        `

        datatable = $('#logTable').DataTable({
            searchDelay: 500,
            processing: true,
            serverSide: true,
            paging: false,
            responsive: true,
            order: [[0, 'desc']],
            dom: 'ilrtp',
            ajax: {
                url: '/api/log',
                type: 'POST',
                data: function (d) {
                    d.period = periodEl.value;
                    d.userId = userEl.value;
                    d.taskId = taskEl.value;
                }
            },
            columns: [
                {
                    data: 'timeStamp',
                    type: 'date'
                },
                {
                    data: 'priorityName',
                    width: '100',
                    render: function (data, type, row) {
                        var color = "info";
                        if (row.priorityName == "ERR") {
                            color = "danger";
                        }
                        if (row.priorityName == "ALERT") {
                            color = "dark";
                        }

                        return `
                            <span class="badge text-bg-${color}">${row.priorityName}</span >
                        `;

                    }
                },
                {
                    data: 'message',
                    orderable: false
                },
                {
                    data: 'source'
                },
                {
                    data: 'url',
                    orderable: false,
                    render: function (data, type, row) {
                        return `
                            <span class="d-inline-block text-truncate" style="max-width: 250px;">
                                ${row.url}
                            </span>
                        `;
                    }
                },
                {
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        return `
                            <a class="detail" data-bs-toggle="modal" data-bs-target="#detailDialog" data-id="${row.id}">
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-list" viewBox="0 0 16 16">
                                    <path fill-rule="evenodd" d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5z"/>
                                </svg>
                            </a>
                        `;
                    }
                }
            ],
        });

        datatable.on('draw', function () {

        });

        const refreshButton = document.getElementById("refreshButton");
        refreshButton.addEventListener('click', (event) => {
            event.preventDefault();

            datatable.ajax.reload();
        });

        const interval = document.getElementById("interval");
        timer = setInterval(function () {
            datatable.ajax.reload();
        }, 1000 * interval.value);

        interval.addEventListener('change', (event) => {
            clearInterval(timer);
            timer = setInterval(function () {
                datatable.ajax.reload();
            }, 1000 * interval.value);
        });

        const pauseButton = document.getElementById("pauseButton");
        pauseButton.addEventListener('click', (event) => {
            event.preventDefault();

            if (isPaused) {
                timer = setInterval(function () {
                    datatable.ajax.reload();
                }, 1000 * interval.value);

                pauseButton.innerHTML = pauseIcon;
                isPaused = false;

            } else {
                clearInterval(timer);

                pauseButton.innerHTML = playIcon;
                isPaused = true;
            }
        });
    };


    var refreshLog = function (minutes) {
        console.log('refreshLog(' + minutes + ')');

        var params = {
            period: minutes
        };
        var url = "/api/log" + formatParams(params);

        const xhttp = new XMLHttpRequest();
        xhttp.onload = function () {
            var data = JSON.parse(this.responseText);
            renderLog(data);
        }
        xhttp.open("GET", url, true);
        xhttp.send();
    };

    var initFilters = function () {
        const periodEl = document.getElementById("period");

        periodEl.addEventListener('change', (event) => {

            datatable.ajax.reload();

        });

        const search = document.getElementById('searchInput');
        search.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });

        const taskEl = document.getElementById("TaskId");
        taskEl.addEventListener('change', (event) => {
            datatable.ajax.reload();
        });

        const userEl = document.getElementById("UserId");
        userEl.addEventListener('change', (event) => {
            datatable.ajax.reload();
        });

        const clearBtn = document.getElementById("clearFilter");
        clearBtn.addEventListener('click', (event) => {
            event.preventDefault();

            search.value = "";
            userEl.value = "0";
            taskEl.value = "00000000-0000-0000-0000-000000000000";

            datatable.ajax.reload();
        });
    };

    var initDialogs = function () {
        const detailDialog = document.getElementById('detailDialog');
        const detailBody = document.getElementsByClassName('modal-body')[0];
        detailDialog.addEventListener('show.bs.modal', event => {

            axios.get('/api/log/' + event.relatedTarget.dataset.id)
                .then(function (response) {
                    console.log(response);
                    if (response.data.detail != null) {
                        detailBody.innerHTML = response.data.detail;
                    } else {
                        detailBody.innerHTML = 'No detail';
                    }

                })
                .catch(function (error) {
                    console.log(error);
                });
        });
    };

    var escapeHTML = function (html) {
        return html.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }


    return {
        init: function () {
            initDatatable();
            initDialogs();
            initFilters();
        }
    };

}();

document.addEventListener("DOMContentLoaded", function () {
    LogDatable.init();
})
