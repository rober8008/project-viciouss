function AdminReportTypeViewModel() {
    var self = this;
    self.reportTypeCollection = ko.observableArray();
    self.creatingMode = false;    

    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "/AdminReportTypes/GetReportTypes",
        }).done(function (data) {
            $(data).each(function (index, element) {
                var mappedItem =
                    {
                        Id: ko.observable(element.Id),
                        name: ko.observable(element.name),
                        active: ko.observable(element.active),
                        Mode: ko.observable("display")
                    };
                self.reportTypeCollection.push(mappedItem);
            });
        }).error(function (ex) {
            alert("Error getting Tenants");
        });
    });

    $(document).on("click", ".kout-create", null, function (ev) {
        if (!self.creatingMode) {
            var newItem = {
                Id: ko.observable(0),
                name: ko.observable(),
                active: ko.observable(false),
                Mode: ko.observable("edit")
            }
            self.reportTypeCollection.push(newItem);
            self.creatingMode = true;
        }
    });

    $(document).on("click", ".kout-edit", null, function (ev) {
        var current = ko.dataFor(this);
        current.Mode("edit");
    });

    $(document).on("click", ".kout-update", null, function (ev) {
        var current = ko.dataFor(this);
        saveData(current);
        current.Mode("display");
        self.creatingMode = false;
    });

    $(document).on("click", ".kout-cancel", null, function (ev) {
        var current = ko.dataFor(this);
        current.Mode("display");
        self.creatingMode = false;
    });

    function saveData(currentData) {
        var postUrl = "/AdminReportTypes/Edit";

        var submitData = {
            Id: currentData.Id(),
            name: currentData.name(),
            active: currentData.active(),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        };

        $.ajax({
            async: false,
            type: "POST",
            contentType: "application/x-www-form-urlencoded",
            url: postUrl,
            data: submitData
        }).done(function (response) {
            if (response.Status === 'OK') {
                currentData.Id(response.ItemId);
            }
            else {
                alert("ERROR Saving");
            }
        }).error(function (response) {
            alert("ERROR Saving");
        })
    }
};

$(function () {
    var adminReportTypeViewModel = new AdminReportTypeViewModel();
    ko.applyBindings(adminReportTypeViewModel, $("#admin-report-type-container")[0]);    
});