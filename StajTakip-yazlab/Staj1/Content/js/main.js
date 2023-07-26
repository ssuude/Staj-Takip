(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();

    $('.message a').click(function () {
        $('form').animate({ height: "toggle", opacity: "toggle" }, "slow");
    });


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Sidebar Toggler
    $('.sidebar-toggler').click(function () {
        $('.sidebar, .content').toggleClass("open");
        return false;
    });


    // Progress Bar
    $('.pg-bar').waypoint(function () {
        $('.progress .progress-bar').each(function () {
            $(this).css("width", $(this).attr("aria-valuenow") + '%');
        });
    }, { offset: '80%' });


    // Calender
    $('#calender').datetimepicker({
        inline: true,
        format: 'L'
    });


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        items: 1,
        dots: true,
        loop: true,
        nav: false
    });


    // Worldwide Sales Chart
    var ctx1 = $("#worldwide-sales").get(0).getContext("2d");
    var myChart1 = new Chart(ctx1, {
        type: "bar",
        data: {
            labels: ["2016", "2017", "2018", "2019", "2020", "2021", "2022"],
            datasets: [{
                label: "USA",
                data: [15, 30, 55, 65, 60, 80, 95],
                backgroundColor: "rgba(0, 156, 255, .7)"
            },
            {
                label: "UK",
                data: [8, 35, 40, 60, 70, 55, 75],
                backgroundColor: "rgba(0, 156, 255, .5)"
            },
            {
                label: "AU",
                data: [12, 25, 45, 55, 65, 70, 60],
                backgroundColor: "rgba(0, 156, 255, .3)"
            }
            ]
        },
        options: {
            responsive: true
        }
    });


    // Salse & Revenue Chart
    var ctx2 = $("#salse-revenue").get(0).getContext("2d");
    var myChart2 = new Chart(ctx2, {
        type: "line",
        data: {
            labels: ["2016", "2017", "2018", "2019", "2020", "2021", "2022"],
            datasets: [{
                label: "Salse",
                data: [15, 30, 55, 45, 70, 65, 85],
                backgroundColor: "rgba(0, 156, 255, .5)",
                fill: true
            },
            {
                label: "Revenue",
                data: [99, 135, 170, 130, 190, 180, 270],
                backgroundColor: "rgba(0, 156, 255, .3)",
                fill: true
            }
            ]
        },
        options: {
            responsive: true
        }
    });



    // Single Line Chart
    var ctx3 = $("#line-chart").get(0).getContext("2d");
    var myChart3 = new Chart(ctx3, {
        type: "line",
        data: {
            labels: [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150],
            datasets: [{
                label: "Salse",
                fill: false,
                backgroundColor: "rgba(0, 156, 255, .3)",
                data: [7, 8, 8, 9, 9, 9, 10, 11, 14, 14, 15]
            }]
        },
        options: {
            responsive: true
        }
    });


    // Single Bar Chart
    var ctx4 = $("#bar-chart").get(0).getContext("2d");
    var myChart4 = new Chart(ctx4, {
        type: "bar",
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: {
            responsive: true
        }
    });


    // Pie Chart
    var ctx5 = $("#pie-chart").get(0).getContext("2d");
    var myChart5 = new Chart(ctx5, {
        type: "pie",
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: {
            responsive: true
        }
    });


    // Doughnut Chart
    var ctx6 = $("#doughnut-chart").get(0).getContext("2d");
    var myChart6 = new Chart(ctx6, {
        type: "doughnut",
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: {
            responsive: true
        }
    });

    //---------------------------------TABLE-------------------------------
    $(document).ready(function () {
        //Only needed for the filename of export files.
        //Normally set in the title tag of your page.
        document.title = 'Simple DataTable';
        // DataTable initialisation
        $('#example').DataTable(
            {
                "dom": '<"dt-buttons"Bf><"clear">lirtp',
                "paging": false,
                "autoWidth": true,
                "columnDefs": [
                    { "orderable": false, "targets": 5 }
                ],
                "buttons": [
                    'colvis',
                    'copyHtml5',
                    'csvHtml5',
                    'excelHtml5',
                    'pdfHtml5',
                    'print'
                ]
            }
        );
        //Add row button
        $('.dt-add').each(function () {
            $(this).on('click', function (evt) {
                //Create some data and insert it
                var rowData = [];
                var table = $('#example').DataTable();
                //Store next row number in array
                var info = table.page.info();
                rowData.push(info.recordsTotal + 1);
                //Some description
                rowData.push('New Order');
                //Random date
                var date1 = new Date(2016, 01, 01);
                var date2 = new Date(2018, 12, 31);
                var rndDate = new Date(+date1 + Math.random() * (date2 - date1));//.toLocaleDateString();
                rowData.push(rndDate.getFullYear() + '/' + (rndDate.getMonth() + 1) + '/' + rndDate.getDate());
                //Status column
                rowData.push('NEW');
                //Amount column
                rowData.push(Math.floor(Math.random() * 2000) + 1);
                //Inserting the buttons ???
                rowData.push('<button type="button" class="btn btn-primary btn-xs dt-edit" style="margin-right:16px;"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button><button type="button" class="btn btn-danger btn-xs dt-delete"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></button>');
                //Looping over columns is possible
                //var colCount = table.columns()[0].length;
                //for(var i=0; i < colCount; i++){			}

                //INSERT THE ROW
                table.row.add(rowData).draw(false);
                //REMOVE EDIT AND DELETE EVENTS FROM ALL BUTTONS
                $('.dt-edit').off('click');
                $('.dt-delete').off('click');
                //CREATE NEW CLICK EVENTS
                $('.dt-edit').each(function () {
                    $(this).on('click', function (evt) {
                        $this = $(this);
                        var dtRow = $this.parents('tr');
                        $('div.modal-body').innerHTML = '';
                        $('div.modal-body').append('Row index: ' + dtRow[0].rowIndex + '<br/>');
                        $('div.modal-body').append('Number of columns: ' + dtRow[0].cells.length + '<br/>');
                        for (var i = 0; i < dtRow[0].cells.length; i++) {
                            $('div.modal-body').append('Cell (column, row) ' + dtRow[0].cells[i]._DT_CellIndex.column + ', ' + dtRow[0].cells[i]._DT_CellIndex.row + ' => innerHTML : ' + dtRow[0].cells[i].innerHTML + '<br/>');
                        }
                        $('#myModal').modal('show');
                    });
                });
                $('.dt-delete').each(function () {
                    $(this).on('click', function (evt) {
                        $this = $(this);
                        var dtRow = $this.parents('tr');
                        if (confirm("Are you sure to delete this row?")) {
                            var table = $('#example').DataTable();
                            table.row(dtRow[0].rowIndex - 1).remove().draw(false);
                        }
                    });
                });
            });
        });
        //Edit row buttons
        $('.dt-edit').each(function () {
            $(this).on('click', function (evt) {
                $this = $(this);
                var dtRow = $this.parents('tr');
                $('div.modal-body').innerHTML = '';
                $('div.modal-body').append('Row index: ' + dtRow[0].rowIndex + '<br/>');
                $('div.modal-body').append('Number of columns: ' + dtRow[0].cells.length + '<br/>');
                for (var i = 0; i < dtRow[0].cells.length; i++) {
                    $('div.modal-body').append('Cell (column, row) ' + dtRow[0].cells[i]._DT_CellIndex.column + ', ' + dtRow[0].cells[i]._DT_CellIndex.row + ' => innerHTML : ' + dtRow[0].cells[i].innerHTML + '<br/>');
                }
                $('#myModal').modal('show');
            });
        });
        //Delete buttons
        $('.dt-delete').each(function () {
            $(this).on('click', function (evt) {
                $this = $(this);
                var dtRow = $this.parents('tr');
                if (confirm("Are you sure to delete this row?")) {
                    var table = $('#example').DataTable();
                    table.row(dtRow[0].rowIndex - 1).remove().draw(false);
                }
            });
        });
        $('#myModal').on('hidden.bs.modal', function (evt) {
            $('.modal .modal-body').empty();
        });
    });



})(jQuery);

