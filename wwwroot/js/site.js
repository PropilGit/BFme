function Test(id) {

    $.post("/Home/GetInfo", { id: id })
        .done(function (data) {

            var count = data.length;
            console.log(count);

            var result_str = "";
            for (var i = 0; i < count; i++) {
                console.log(data[i]);
                result_str = result_str + " " + data[i];
            }
            $("#result_text").html(result_str);
            console.log(data);
        });
}



function DeleteInvestConcept(id) {
    $.post("/InvestConcept/Remove", { Id: id })
        .done(function (result) {
            if (result) {
                RemoveInvestConcept(id);
            }
        });
}

function RemoveInvestConcept(id) {
    var selector = "#InvestConcept_" + id;
    $(selector).detach();
}



function UpdateExpenses(Id) {

    $.post("/InvestConcept/GetExpenses", { Id: Id })
        .done(function (data) {
            InsertExpenses(data);
        });
}

function InsertExpenses(exps) {

    var tableId = "#ExpensesTable_" + exps[0].investConceptId;
    var count = exps.length;

    var result = "<table><tr><td>Категория расхода</td><td>Разово</td><td>Ежемесячно</td></tr>";

    for (var i = 0; i < count; i++) {
        result += "<tr><td>" + exps[i].name + "</td><td>" + exps[i].singlePayment + "</td><td>" + exps[i].monthlyPayment + "</td></tr>"
    }
    result += "</table>"
    console.log(result);
    $(tableId).html(result);
}

function AddExpense(Id) {

    //var data = $(formId).serializeArray();
    var InvestConceptId = $("#InvestConceptId_ExpenseForm_" + Id).val();
    var Name = $("#Name_ExpenseForm_" + Id).val()
    var SinglePayment = $("#SinglePayment_ExpenseForm_" + Id).val()
    var MonthlyPayment = $("#MonthlyPayment_ExpenseForm_" + Id).val()

    $.post("/InvestConcept/AddExpense", { InvestConceptId: InvestConceptId, Name: Name, SinglePayment: SinglePayment, MonthlyPayment: MonthlyPayment})
        .done(function (result) {
            if (result) {

                $("#Name_ExpenseForm_" + Id).val('');
                $("#SinglePayment_ExpenseForm_" + Id).val(0);
                $("#MonthlyPayment_ExpenseForm_" + Id).val(0);

                UpdateExpenses(Id);
            }
        });
}



