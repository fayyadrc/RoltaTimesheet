//Form toggle:



$(document).ready(function () {
    // Function to toggle the form
    function toggleForm() {
        var roundedBox = $(this).closest(".rounded-box");
        var formContainer = roundedBox.next(".form-container");
        formContainer.toggleClass('hidden');

        var expandButton = roundedBox.find("i.expand-button");
        expandButton.toggleClass('fa-plus', formContainer.hasClass('hidden'));
        expandButton.toggleClass('fa-minus', !formContainer.hasClass('hidden'));
    }

    // Event handler for clicking on the rounded box or the icons
    $(document).on("click", ".rounded-box, .expand-button", toggleForm);

    
});


// Function to calculate total hours from the table
function calculateTotalHours(tableId) {
    var totalHours = 0;
    $("#" + tableId + " tbody tr").each(function () {
        var hours = parseFloat($(this).find("td:nth-child(3)").text()); // Parse hours as a floating-point number
        if (!isNaN(hours)) {
            totalHours += hours;
        }
    });
    return totalHours;
}

function updateTotalBillableHours() {
    var totalBillableHours = 0;
    $("#billableData tbody tr").each(function () {
        var hours = parseFloat($(this).find("td:nth-child(6)").text()); // Parse hours as a floating-point number
        if (!isNaN(hours)) {
            totalBillableHours += hours;
        }
    });
    $("#billableData .total-hours").text(totalBillableHours);
}

// Function to update total hours below each table
function updateTotalHours(tableId) {
    var totalHours = calculateTotalHours(tableId);
    $("#" + tableId + " .total-hours").text(totalHours);
}

$(document).ready(function () {
    // Calculate and display the total billable hours when the page loads
    updateTotalBillableHours();
});



//Billable Data
var billablejsonData = [];

let uniqueBillableID = 1;
function addBillableData() {
    var obj = {
        id: uniqueBillableID++,
        projectNumber: "",
        projectName: "",
        customer: "",
        segment: "",
        monthYear: "",
        hours: ""
        
    };

    var formContainer = $(".billableFormContainer");
    var projectNumber = formContainer.find("#projectNumber").val();
    var projectName = formContainer.find("#projectName option:selected").text();
    var customer = formContainer.find("#projectCustomer option:selected").text();
    var customerid = formContainer.find("#projectCustomer option:selected").val();
    var segment = formContainer.find("#segment option:selected").text();
    var segmentid = formContainer.find("#segment option:selected").val();
    var monthYear = $("#selectedMonth").val();
    var hours = formContainer.find("#hours").val();


    

    //Check if any needed field is empty
    if (!projectNumber || !projectName || !monthYear || !hours) {
        
        alert("Please fill all required fields.");
        return;
    }

    if (projectNumber && projectName && monthYear && hours) {
        // Create a new row and cells
        var newRow = $("<tr>");
        newRow.attr("data-id", obj.id);
        var projectNumberCell = $("<td>").text(projectNumber);
        var projectNameCell = $("<td>").text(projectName);
        var customerCell = $("<td>").text(customer);
        var segmentCell = $("<td>").text(segment);
        var monthYearCell = $("<td>").text(monthYear);
        var hoursCell = $("<td>").text(parseFloat(hours).toFixed(2));


        // Create the trash icon and add it to a cell
        var trashIcon = $("<i>").addClass("fa fa-trash").css("color", "#1d4d8f");
        var deleteCell = $("<td>").append(trashIcon).addClass("center-icon");


        // Append cells to the row
        newRow.append(
            projectNumberCell,
            projectNameCell,
            customerCell,
            segmentCell,
            monthYearCell,
            hoursCell,
            deleteCell
        );

        // Append the row to the top of the billable table body
        $("#billableData tbody").prepend(newRow);

        // Handle the delete button click event
        trashIcon.on("click", function () {
            // Remove the row from the table
            newRow.remove();
            var idToDelete = newRow.attr("data-id");
            var indexToDelete = billablejsonData.findIndex(item => item.id === parseInt(idToDelete));
            if (indexToDelete !== -1) {
                billablejsonData.splice(indexToDelete, 1);
                /*console.log("Deleted from json:", billablejsonData);*/
            } else {
                
            }

            // Update the total billable hours after adding a row
            updateTotalBillableHours();
        });

        // Add data to JSON
        obj.projectNumber = projectNumber;
        obj.projectName = projectName;
        obj.customer = customer;
        obj.customerid = customerid;
        obj.segmentid = segmentid;
        obj.segment = segment;
        obj.monthYear = monthYear;
        obj.hours = hours;
        billablejsonData.push(obj);
        /*console.log(billablejsonData);*/

        // Clear input fields after adding a row
        $("#projectNumber").val("");
        $("#projectName").val("");
        $("#projectCustomer").val("");
        $("#segment").val("");
        $("#month").val("");
        $("#hours").val("");
    } else {
        alert("Please fill all required fields.");
    }
    updateTotalBillableHours();

}
function calculateBillableHours() {
    var totalBillableHours = 0;
    for (var i = 0; i < billablejsonData.length; i++) {
        totalBillableHours += parseFloat(billablejsonData[i].hours); // Parse hours as a floating-point number
    }
    return totalBillableHours;
}



//Non Billable and Office Support data (JSON)
var nonBillablejsonData = [];
var officeSupportjsonData = [];

function addData(formContainerElement, tableId) {
    var obj = {
        projectName: "",
        monthYear: "",
        hours: ""
    };
     

    var formContainer = $(formContainerElement);
    var projectNumber = formContainer.find("#projectName").val();
    
    console.log("projectNumber:", projectNumber);
    var projectName = formContainer.find("#projectName option:selected").text();
    var monthYear = $("#selectedMonth").val();
    var hours = formContainer.find("#hours").val();





    if (!projectName || !monthYear || !hours) {
        alert("Please fill all required fields.");
        return;
    }


    if (projectName && monthYear && !isNaN(hours)) {
        // Create a new row and cells
        var newRow = $("<tr>");
        var projectNameCell = $("<td>").text(projectName);
        var monthYearCell = $("<td>").text(monthYear);
        var hoursCell = $("<td>").text(parseInt(hours));

        // Create the trash icon and add it to a cell
        var trashIcon = $("<i>").addClass("fa fa-trash").css("color", "#1d4d8f", "display", "block", "textAlign", "center");
        var deleteCell = $("<td>").append(trashIcon);

        // Append cells to the row
        newRow.append(projectNameCell, monthYearCell, hoursCell, deleteCell);

        // Append the row to the top of the correct table using
        $("#" + tableId + " tbody").prepend(newRow);

        // Function of the trash icon
        trashIcon.on("click", function () {
            // Remove the row from the table
            newRow.remove();

            // Update the total hours after removing a row
            updateTotalHours(tableId);

            // Remove data from the specific JSON file based on the tableId
            var indexToDelete = -1;
            var jsonData;
            if (tableId === "billableData") {
                jsonData = billablejsonData;
            } else if (tableId === "nonBillableData") {
                jsonData = nonBillablejsonData;
            } else if (tableId === "officeSupportData") {
                jsonData = officeSupportjsonData;
            }

            // Find the index of the item in the JSON data that matches the row data
            $(jsonData).each(function (index, item) {
                if (item.projectName === projectName && item.monthYear === monthYear && item.hours === hours) {
                    indexToDelete = index;
                    return false; // Exit the loop when the item is found
                }
            });

            // Remove the item from the JSON data and update the respective JSON file
            if (indexToDelete !== -1) {
                jsonData.splice(indexToDelete, 1);
                console.log("Deleted from " + tableId + " jsonData:", jsonData);
            }
        });

        // Update the total hours after adding a row
        updateTotalHours(tableId);

        // Add data to JSON
        obj.projectNumber = projectNumber;
        obj.projectName = projectName;
        obj.monthYear = monthYear;
        obj.hours = hours;

        if (tableId === "billableData") {
            billablejsonData.push(obj);
            updateTotalBillableHours(); 
        } else if (tableId === "nonBillableData") {
            nonBillablejsonData.push(obj);
            updateTotalHours("nonBillableData"); 
        } else if (tableId === "officeSupportData") {
            officeSupportjsonData.push(obj);
            updateTotalHours("officeSupportData"); 
        }

        // Clear input fields after adding a row
        $("select[name=name]", formContainerElement).val("");
        $("input[name=month]", formContainerElement).val("");
        $("input[name=hours]", formContainerElement).val("");
    } else {
        alert("Please fill all required fields.");
    }
}



$(".submitallforms").on("click", function () {
    submitAllForms();
});


function submitAllForms() {
    
    
    var totalBillableHours = calculateBillableHours();
    var totalNonBillableHours = calculateTotalHours('nonBillableData');
    var totalOfficeSupportHours = calculateTotalHours('officeSupportData');
    
    
    $(".modalInfo h2").eq(0).text("Total Hours: " + (totalBillableHours + totalNonBillableHours + totalOfficeSupportHours));

   
    var utilization = calculateUtilization(totalBillableHours, totalBillableHours + totalNonBillableHours + totalOfficeSupportHours);
    $(".modalInfo h2").eq(1).text("Utilization: " + utilization + "%");


   
    var billableTableBody = $("#billableDataModal tbody");
    billableTableBody.empty(); 

    billablejsonData.forEach(function (item) {
        var row = $("<tr>");
        row.append($("<td>").text(item.projectNumber));
        row.append($("<td>").text(item.projectName));
        row.append($("<td>").text(item.customer));
        row.append($("<td>").text(item.segment));
        row.append($("<td>").text(item.monthYear));
        row.append($("<td>").text(item.hours));
        billableTableBody.append(row);
    });

   

    var nonBillableTableBody = $("#nonBillableDataModal tbody");
    nonBillableTableBody.empty(); 

    nonBillablejsonData.forEach(function (item) {
        var row = $("<tr>");
        row.append($("<td>").text(item.projectNumber));
        row.append($("<td>").text(item.projectName));
        row.append($("<td>").text(item.monthYear));
        row.append($("<td>").text(item.hours));
        nonBillableTableBody.append(row);
    });

    
    var officeSupportTableBody = $("#officeSupportDataModal tbody");
    officeSupportTableBody.empty(); 

    officeSupportjsonData.forEach(function (item) {
        var row = $("<tr>");
        row.append($("<td>").text(item.projectNumber));
        row.append($("<td>").text(item.projectName));
        row.append($("<td>").text(item.monthYear));
        row.append($("<td>").text(item.hours));
        officeSupportTableBody.append(row);
    });
    $("#dataModal").modal('show');
}


$(".modal-header .close").on("click", function () {
    $("#dataModal").modal("hide");
})


function calculateUtilization(billableHours, totalHours) {
    if (totalHours === 0) {
        return 0; 
    }

    //totalhours is 176 (one month)
    const utilizationPercentage = ((billableHours / 176) * 100).toFixed(2);
    return utilizationPercentage;

}

//autofill data by projectNumber
$(document).ready(function () {

    $("#projectName").on("change", function () {
        var selectedProjectNumber = $(this).val();
        $("#projectNumber").val(selectedProjectNumber);
        $("#projectNumber").prop("disabled", true);
        

        
        $.getJSON(`/api/TimesheetAPI/GetProjectInfoByProNo?projectNumber=${selectedProjectNumber}`)
            .done(function (data) {
                

                if (data !== null) {
                    

                    
                    $("#projectCustomer").val(data.CustomerID);
                    $("#segment").val(data.SegmentID);

                    $("#projectCustomer").prop("disabled", true);
                    $("#segment").prop("disabled", true);
                } else {
                    
                }
            })
            .fail(function (error) {
                
            });
    });
});

//month/year restrictions
$(document).ready(function () {

   
  
    var currentDate = new Date();

    
    var prevMonth = new Date(currentDate);
    prevMonth.setMonth(currentDate.getMonth() - 1);

    
    var prevMonthFormatted = formatDate(prevMonth);
    $("#selectedMonth").attr("min", prevMonthFormatted);
    $("#selectedMonth").attr("max", prevMonthFormatted);

    
    $("#selectedMonth").val(prevMonthFormatted);
});


function formatDate(date) {
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0');
    return year + "-" + month;
}


    //submit button in modal popup 
function InsertData() {
    const mainData = {
        billableData: billablejsonData.map(item => ({
            projectNumber: item.projectNumber,
            projectID: item.projectID,
            customerid: item.customerid,
            segmentid: item.segmentid,
            monthYear: item.monthYear,
            hours: item.hours
        })),
        nonBillableData: nonBillablejsonData.map(item => ({
            projectNumber: item.projectNumber,
            monthYear: item.monthYear,
            hours: item.hours
        })),
        officeSupportData: officeSupportjsonData.map(item => ({
            projectNumber: item.projectNumber,
            monthYear: item.monthYear,
            hours: item.hours
        }))
    };

    $.ajax({
        type: "POST",
        url: "/api/TimesheetAPI/InsertData",
        data: JSON.stringify(mainData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.success) {
                alert(response.message);
                location.reload();
            } else {
                alert("Data could not be saved: " + response.message);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert("An error occurred: " + xhr.responseText);
            console.log("Error:", xhr);
        }
    });

   
    console.log("Final Submission:", mainData);
}
    
