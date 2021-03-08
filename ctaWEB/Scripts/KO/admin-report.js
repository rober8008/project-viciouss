/*
ko.dataFor($("#beneficiariesFormControl")[0]).getBeneficiaries(1000);

-    Get the ViewModel binded to the html item

-    Invoke the method “getBeneficiaries”  declared within it

Use it inside ko.computed to get ReportTypeName
*/
function AdminReportViewModel() {
    var self = this;

    self.reportCollection = ko.observableArray();
    self.reportTypeCollection = ko.observableArray();
    self.reportActiveFilter = ko.observable();
    self.reportTypeFilter = ko.observable();
    self.filterReports = function (filter) {
        self.reportActiveFilter(filter.reportActiveFilterValue);
        self.reportTypeFilter(filter.reportTypeFilterValue);
    };
    self.getReportTypeName = function (typeId) {
        var res = ko.utils.arrayFirst(self.reportTypeCollection(), function (item) {
            return item.Id() === typeId;
        });
        return (res) ? res.name() : '';
    };
    self.reportFileteredCollection = ko.computed(function () {
        return ko.utils.arrayFilter(self.reportCollection(), function (report) {
            return (
                !self.reportTypeFilter() || self.reportTypeFilter() == report.type()) &&
                (!self.reportActiveFilter() || (self.reportActiveFilter() == 'active' && report.active()) || (self.reportActiveFilter() == 'inactive' && !report.active())
                );
        });
    });
    self.creatingMode = false;

    $(document).ready(function () {
        $.get("/AdminReportTypes/GetReportTypes", function (data) {
            $(data).each(function (index, element) {
                var mappedItem =
                    {
                        Id: ko.observable(element.Id),
                        name: ko.observable(element.name)
                    };
                self.reportTypeCollection.push(mappedItem);
            });
        });

        $.get("/AdminReports/GetReports", function (data) {
            $(data).each(function (index, element) {
                var mappedItem =
                    {
                        Id: ko.observable(element.Id),
                        title: ko.observable(element.title),
                        description: ko.observable(element.description),
                        type: ko.observable(element.type),
                        url: ko.observable(element.url),
                        active: ko.observable(element.active),
                        Mode: ko.observable("display")
                    };

                mappedItem.typename = ko.dependentObservable(function () {
                    var typename = ko.utils.arrayFirst(self.reportTypeCollection(), function (report) {
                        return (mappedItem.type() == report.Id());
                    });

                    return typename ? typename.name() : "";                        
                });
                mappedItem.reportTypeCollection = ko.observableArray(self.reportTypeCollection());

                self.reportCollection.push(mappedItem);
            });
        });

        self.reportTypeFilter(false);
        self.reportActiveFilter(false);        
    });

    $(document).on("click", ".kout-create", null, function (ev) {
        if (!self.creatingMode) {
            var newItem = {
                Id: ko.observable(0),
                title: ko.observable(''),
                description: ko.observable(''),
                url: ko.observable(''),
                type: ko.observable(),
                active: ko.observable(false),
                Mode: ko.observable("edit"),
                reportTypeCollection: ko.observableArray(self.reportTypeCollection())                
            }

            newItem.typename = ko.dependentObservable(function () {
                var typename = ko.utils.arrayFirst(self.reportTypeCollection(), function (report) {
                    return (newItem.type() == report.Id());
                });

                return typename ? typename.name() : "";
            })

            self.reportCollection.push(newItem);
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
        var postUrl = "/AdminReports/Edit";

        var submitData = {
            Id: currentData.Id(),
            title: currentData.title(),
            description: currentData.description(),
            url: currentData.url(),
            type: currentData.type(),
            active: currentData.active(),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        };

        $.ajax({
            type: "POST",
            contentType: "application/x-www-form-urlencoded",
            url: postUrl,
            data: submitData
        }).done(function (response) {
            if (response.Status === 'OK') {
                currentData.Id(response.ItemId);
                alert("Report Updated");
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
    var adminReportViewModel = new AdminReportViewModel();
    ko.applyBindings(adminReportViewModel, $("#admin-report-container")[0]);
});

