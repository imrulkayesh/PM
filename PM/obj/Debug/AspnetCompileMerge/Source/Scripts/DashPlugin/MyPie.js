function MakeMy3DPieChart(url, PieName, divId) {
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r);

                //Pie
                var options = {
                    title: PieName,
                    is3D: true
                };
                var chart = new google.visualization.PieChart($(divId)[0]);
                chart.draw(data, options);
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    };
}
function MakeMyHolePieChart(url, PieName, divId) {
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r);

                //Pie
                var options = {
                    title: PieName,
                    pieHole: 0.4
                };
                var chart = new google.visualization.PieChart($(divId)[0]);
                chart.draw(data, options);
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    }
}

function MakeMySlicePieChart(url, PieName, divId) {
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r);

                //Pie
                var options = {
                    title: PieName,
                    slices: {
                        1: { offset: 0.2 },
                        2: { offset: 0.2 },
                        3: { offset: 0.2 },
                    }
                };
                var chart = new google.visualization.PieChart($(divId)[0]);
                chart.draw(data, options);
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    }
}

function MakeMyBarChart(url, PieName, divId) {
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r);

                //Pie
                var options = {
                    title: PieName
                };
                var chart = new google.visualization.BarChart($(divId)[0]);
                chart.draw(data, options);
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    }
}
function MakeMyColumnChart(url, PieName, divId) {
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r);

                //Pie
                var options = {
                    title: PieName
                };
                var chart = new google.visualization.ColumnChart($(divId)[0]);
                chart.draw(data, options);
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    }
}


function MakeMyMaterialBarChart(url, PieName, BarSubTitle, divId) {
    google.charts.load('current', { 'packages': ['bar'] });
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r);

                //Pie
                var options = {
                    title: PieName,
                    subtitle: BarSubTitle,
                };
                var chart = new google.charts.Bar(document.getElementById(divId));
                chart.draw(data, google.charts.Bar.convertOptions(options));
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    };
}
