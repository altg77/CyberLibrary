// Function to fetch and update dashboard counts
async function updateDashboardCounts() {
    try {
        const response = await fetch('/Dashboard/GetDashboardCounts');
        const data = await response.json();

        document.getElementById('activeLoansCount').innerText = data.activeLoans;
        document.getElementById('usersCount').innerText = data.users;
        document.getElementById('availableBooksCount').innerText = data.availableBooks;
        document.getElementById('categoriesCount').innerText = data.categories;

    } catch (error) {
        console.error('Error fetching dashboard counts:', error);
        document.getElementById('activeLoansCount').innerText = 'N/A';
        document.getElementById('usersCount').innerText = 'N/A';
        document.getElementById('availableBooksCount').innerText = 'N/A';
        document.getElementById('categoriesCount').innerText = 'N/A';
    }
}

// Function to render the Area Chart (Recent Loans)
async function renderAreaChart() {
    try {
        const response = await fetch('/Dashboard/GetRecentLoansChartData');
        const data = await response.json();

        const labels = data.map(item => item.Month);
        const counts = data.map(item => item.Count);

        var ctx = document.getElementById("myAreaChart");
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: "Empréstimos",
                    lineTension: 0.3,
                    backgroundColor: "rgba(0,123,255,0.2)",
                    borderColor: "rgba(0,123,255,1)",
                    pointRadius: 5,
                    pointBackgroundColor: "rgba(0,123,255,1)",
                    pointBorderColor: "rgba(255,255,255,0.8)",
                    pointHoverRadius: 5,
                    pointHoverBackgroundColor: "rgba(0,123,255,1)",
                    pointHitRadius: 50,
                    pointBorderWidth: 2,
                    data: counts,
                }],
            },
            options: {
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'date'
                        },
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            maxTicksLimit: 7
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            maxTicksLimit: 5
                        },
                        gridLines: {
                            color: "rgba(0, 0, 0, .125)",
                        }
                    }],
                },
                legend: {
                    display: false
                }
            }
        });

    } catch (error) {
        console.error('Error rendering area chart:', error);
    }
}

// Function to render the Bar Chart (Books by Sector)
async function renderBarChart() {
    try {
        const response = await fetch('/Dashboard/GetBooksBySectorChartData');
        const data = await response.json();

        const labels = data.map(item => item.Sector);
        const counts = data.map(item => item.Count);

        var ctx = document.getElementById("myBarChart");
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: "Livros",
                    backgroundColor: "rgba(40,167,69,0.8)",
                    borderColor: "rgba(40,167,69,1)",
                    data: counts,
                }],
            },
            options: {
                scales: {
                    xAxes: [{
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            maxTicksLimit: 6
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            maxTicksLimit: 5
                        },
                        gridLines: {
                            display: true
                        }
                    }],
                },
                legend: {
                    display: false
                }
            }
        });

    } catch (error) {
        console.error('Error rendering bar chart:', error);
    }
}

// Function to initialize the DataTables for recent loans
function initializeRecentLoansTable() {
    $('#datatablesSimple').DataTable({
        ajax: {
            url: '/Dashboard/GetLatestLoans', // Your endpoint to fetch latest loans
            dataSrc: 'data' // DataTables expects the data array within a 'data' property
        },
        columns: [
            { data: 'BookTitle' },
            { data: 'UserName' },
            { data: 'LoanDate' },
            { data: 'ReturnDate' },
            { data: 'Status' }
        ],
        order: [[2, 'desc']] // Order by LoanDate descending by default
    });
}

// On document ready
window.addEventListener('DOMContentLoaded', event => {
    updateDashboardCounts();
    renderAreaChart();
    renderBarChart();
    initializeRecentLoansTable(); // Initialize DataTables with AJAX data
});