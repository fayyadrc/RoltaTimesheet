document.addEventListener("DOMContentLoaded", function () {
    const employeePicker = document.getElementById("employeePicker");
    const dataContainers = [document.getElementById("billableData"), document.getElementById("nonBillableData"), document.getElementById("officeSupportData")];
    const projectLabels = document.querySelectorAll(".collected-data-table h4");

    employeePicker.addEventListener("change", function () {
        const EmployeeID = employeePicker.value;

        if (EmployeeID) {
            fetchAndDisplayData(EmployeeID);
        } else {

        }
    });

    function fetchAndDisplayData(EmployeeID) {
        window.location.href = '/Admin/Dashboard?id=' + EmployeeID;
    }
    // calculate total billable hours and utilization percentage
    const billableHoursCells = document.querySelectorAll('#billableData td:last-child'); 
    let totalBillableHours = 0;
    billableHoursCells.forEach(cell => {
        totalBillableHours += parseFloat(cell.textContent);
    });

    const workHoursPerMonth = 176;
    const utilizationPercentage = (totalBillableHours / workHoursPerMonth) * 100;

    // display %
    const utilizationContainer = document.getElementById('utilizationContainer');
    utilizationContainer.textContent = `Utilization Percentage: ${utilizationPercentage.toFixed(1)}%`;

    const nonBillableHourCells = document.querySelectorAll('#nonBillableData td:last-child'); 
    let totalNonBillableHours = 0;
    nonBillableHourCells.forEach(cell => {
        totalNonBillableHours += parseFloat(cell.textContent);
    });

    const officeSupporthours = document.querySelectorAll('#officeSupportData td:last-child'); 
    let totalOfficeSupportHours = 0;
    officeSupporthours.forEach(cell => {
        totalOfficeSupportHours += parseFloat(cell.textContent);
    });

    const totalHours = totalBillableHours + totalNonBillableHours + totalOfficeSupportHours;
    const totalHoursContainer = document.getElementById('totalHoursContainer');
    totalHoursContainer.textContent = `Total Hours: ${totalHours.toFixed(0)}`;



});