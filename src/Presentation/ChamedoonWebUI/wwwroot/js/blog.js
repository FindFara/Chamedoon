(function (global) {
    var $ = global.jQuery;
    if (!$) {
        return;
    }

    var pluginName = "persianDatepicker";
    if (!$.fn || typeof $.fn[pluginName] !== "function" || typeof global.persianDate !== "function") {
        return;
    }

    var isoToJalali = function (isoValue) {
        if (!isoValue) {
            return "";
        }
        try {
            var normalized = isoValue;
            if (normalized.length === 10) {
                normalized += "T00:00:00";
            }
            var persian = new global.persianDate(new Date(normalized));
            return persian.format("YYYY/MM/DD");
        } catch (error) {
            return "";
        }
    };

    var initializePicker = function ($input) {
        var altSelector = $input.data("altField");
        var $altField = altSelector ? $(altSelector) : null;
        var initialIso = $input.data("initialValue") || ($altField ? $altField.val() : "");

        $input.persianDatepicker({
            format: "YYYY/MM/DD",
            altField: altSelector,
            altFormat: "YYYY-MM-DD",
            initialValue: false,
            autoClose: true,
            calendar: {
                locale: "fa",
                persian: {
                    locale: "fa"
                }
            },
            navigator: {
                enabled: true,
                text: {
                    btnNextText: "ماه بعد",
                    btnPrevText: "ماه قبل"
                }
            },
            toolbox: {
                calendarSwitch: {
                    enabled: false
                },
                todayButton: {
                    enabled: true,
                    text: "امروز"
                }
            }
        });

        if (initialIso) {
            var jalaliValue = isoToJalali(initialIso);
            if (jalaliValue) {
                $input.val(jalaliValue);
            }
        }
    };

    var onReady = function () {
        $(".jalali-date-input").each(function () {
            initializePicker($(this));
        });
    };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", onReady);
    } else {
        onReady();
    }
})(window);
