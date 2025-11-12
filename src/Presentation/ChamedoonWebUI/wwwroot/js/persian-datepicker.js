(function () {
    const PERSIAN_DIGITS = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    const EN_DIGIT_MAP = {
        '۰': '0',
        '۱': '1',
        '۲': '2',
        '۳': '3',
        '۴': '4',
        '۵': '5',
        '۶': '6',
        '۷': '7',
        '۸': '8',
        '۹': '9'
    };
    const MONTH_NAMES = [
        'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
        'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'
    ];
    const WEEKDAY_LABELS = ['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج'];
    const PERSIAN_EPOCH = 1948320;
    const instances = [];

    const toPersianDigits = (value) => value.replace(/\d/g, (digit) => PERSIAN_DIGITS[digit]);
    const toEnglishDigits = (value) => value.replace(/[۰-۹]/g, (digit) => EN_DIGIT_MAP[digit] || digit);
    const padNumber = (num, size) => num.toString().padStart(size, '0');
    const mod = (a, b) => a - b * Math.floor(a / b);

    const gregorianToJdn = (year, month, day) => {
        return (
            Math.floor((1461 * (year + 4800 + Math.floor((month - 14) / 12))) / 4) +
            Math.floor((367 * (month - 2 - 12 * Math.floor((month - 14) / 12))) / 12) -
            Math.floor((3 * Math.floor((year + 4900 + Math.floor((month - 14) / 12)) / 100)) / 4) +
            day - 32075
        );
    };

    const jdnToGregorian = (jdn) => {
        let f = jdn + 1401 + Math.floor((Math.floor((4 * jdn + 274277) / 146097) * 3) / 4) - 38;
        let e = 4 * f + 3;
        let g = Math.floor((e % 1461) / 4);
        let h = 5 * g + 2;
        let day = Math.floor((h % 153) / 5) + 1;
        let month = ((Math.floor(h / 153) + 2) % 12) + 1;
        let year = Math.floor(e / 1461) - 4716 + Math.floor((12 + 2 - month) / 12);
        return { year, month, day };
    };

    const persianToJdn = (year, month, day) => {
        const epBase = year - (year >= 0 ? 474 : 473);
        const epYear = 474 + mod(epBase, 2820);
        return day +
            (month <= 7 ? (month - 1) * 31 : (month - 1) * 30 + 6) +
            Math.floor((epYear * 682 - 110) / 2816) +
            (epYear - 1) * 365 +
            Math.floor(epBase / 2820) * 1029983 +
            (PERSIAN_EPOCH - 1);
    };

    const jdnToPersian = (jdn) => {
        jdn = Math.floor(jdn);
        const depoch = jdn - persianToJdn(475, 1, 1);
        const cycle = Math.floor(depoch / 1029983);
        const cyear = mod(depoch, 1029983);
        let ycycle;
        if (cyear === 1029982) {
            ycycle = 2820;
        } else {
            const aux1 = Math.floor(cyear / 366);
            const aux2 = mod(cyear, 366);
            ycycle = Math.floor((2134 * aux1 + 2816 * aux2 + 2815) / 1028522) + aux1 + 1;
        }
        let year = ycycle + 2820 * cycle + 474;
        if (year <= 0) {
            year -= 1;
        }
        const yday = jdn - persianToJdn(year, 1, 1) + 1;
        const month = yday <= 186 ? Math.ceil(yday / 31) : Math.ceil((yday - 6) / 30);
        const day = jdn - persianToJdn(year, month, 1) + 1;
        return { year, month, day };
    };

    const isPersianLeap = (year) => {
        const epBase = year - (year >= 0 ? 474 : 473);
        const epYear = 474 + mod(epBase, 2820);
        return ((epYear * 682) - 110) % 2816 < 682;
    };

    const persianMonthLength = (year, month) => {
        if (month <= 6) {
            return 31;
        }
        if (month <= 11) {
            return 30;
        }
        return isPersianLeap(year) ? 30 : 29;
    };

    const getTodayPersian = () => {
        const today = new Date();
        const gregorian = {
            year: today.getFullYear(),
            month: today.getMonth() + 1,
            day: today.getDate()
        };
        return jdnToPersian(gregorianToJdn(gregorian.year, gregorian.month, gregorian.day));
    };

    const formatPersianDisplay = (date) => {
        const year = padNumber(date.year, 4);
        const month = padNumber(date.month, 2);
        const day = padNumber(date.day, 2);
        return toPersianDigits(`${year}/${month}/${day}`);
    };

    const formatGregorianHidden = (date) => {
        const year = padNumber(date.year, 4);
        const month = padNumber(date.month, 2);
        const day = padNumber(date.day, 2);
        return `${year}-${month}-${day}`;
    };

    const parseGregorianValue = (value) => {
        if (!value) {
            return null;
        }
        const match = value.trim().match(/^(\d{4})-(\d{1,2})-(\d{1,2})$/);
        if (!match) {
            return null;
        }
        const year = parseInt(match[1], 10);
        const month = parseInt(match[2], 10);
        const day = parseInt(match[3], 10);
        if (month < 1 || month > 12 || day < 1 || day > 31) {
            return null;
        }
        return { year, month, day };
    };

    const parsePersianValue = (value) => {
        if (!value) {
            return null;
        }
        const normalized = toEnglishDigits(value).replace(/\s/g, '');
        const match = normalized.match(/^(\d{4})[\/-](\d{1,2})[\/-](\d{1,2})$/);
        if (!match) {
            return null;
        }
        const year = parseInt(match[1], 10);
        const month = parseInt(match[2], 10);
        const day = parseInt(match[3], 10);
        if (month < 1 || month > 12) {
            return null;
        }
        if (day < 1 || day > persianMonthLength(year, month)) {
            return null;
        }
        return { year, month, day };
    };

    const datesEqual = (a, b) => a && b && a.year === b.year && a.month === b.month && a.day === b.day;

    const closeAll = (exceptInstance) => {
        instances.forEach((instance) => {
            if (instance !== exceptInstance) {
                instance.close();
            }
        });
    };

    const getPersianWeekIndex = (gregorianWeekday) => {
        return gregorianWeekday === 6 ? 0 : gregorianWeekday + 1;
    };

    class DatePicker {
        constructor(input) {
            this.input = input;
            this.hidden = null;
            this.picker = null;
            this.titleElement = null;
            this.gridElement = null;
            this.viewDate = getTodayPersian();
            this.selectedDate = null;
            this.visible = false;
            this.boundReposition = this.positionPicker.bind(this);

            this.resolveHiddenInput();
            this.createPicker();
            this.attachInputEvents();
            this.syncInitialValue();
            instances.push(this);
        }

        resolveHiddenInput() {
            const selector = this.input.getAttribute('data-alt-field');
            if (selector) {
                this.hidden = document.querySelector(selector);
            }
        }

        createPicker() {
            this.picker = document.createElement('div');
            this.picker.className = 'persian-datepicker';

            const header = document.createElement('div');
            header.className = 'persian-datepicker-header';

            const prevButton = document.createElement('button');
            prevButton.type = 'button';
            prevButton.className = 'persian-datepicker-nav';
            prevButton.setAttribute('aria-label', 'ماه قبل');
            prevButton.innerHTML = '&#x2039;';
            prevButton.addEventListener('click', (event) => {
                event.preventDefault();
                this.changeMonth(-1);
            });

            const nextButton = document.createElement('button');
            nextButton.type = 'button';
            nextButton.className = 'persian-datepicker-nav';
            nextButton.setAttribute('aria-label', 'ماه بعد');
            nextButton.innerHTML = '&#x203A;';
            nextButton.addEventListener('click', (event) => {
                event.preventDefault();
                this.changeMonth(1);
            });

            this.titleElement = document.createElement('div');
            this.titleElement.className = 'persian-datepicker-title';

            header.appendChild(prevButton);
            header.appendChild(this.titleElement);
            header.appendChild(nextButton);

            const weekdays = document.createElement('div');
            weekdays.className = 'persian-datepicker-weekdays';
            WEEKDAY_LABELS.forEach((label) => {
                const span = document.createElement('span');
                span.className = 'persian-datepicker-weekday';
                span.textContent = label;
                weekdays.appendChild(span);
            });

            this.gridElement = document.createElement('div');
            this.gridElement.className = 'persian-datepicker-grid';

            this.picker.appendChild(header);
            this.picker.appendChild(weekdays);
            this.picker.appendChild(this.gridElement);

            document.body.appendChild(this.picker);
        }

        attachInputEvents() {
            this.input.addEventListener('focus', () => this.open());
            this.input.addEventListener('click', () => {
                this.open();
            });
            this.input.addEventListener('keydown', (event) => {
                if (event.key === 'ArrowDown' || event.key === 'Enter') {
                    event.preventDefault();
                    this.open();
                } else if (event.key === 'Escape') {
                    this.close();
                }
            });
            this.input.addEventListener('input', () => {
                if (!this.input.value.trim()) {
                    this.clearSelection();
                } else {
                    const parsed = parsePersianValue(this.input.value);
                    if (parsed) {
                        this.selectedDate = parsed;
                        this.viewDate = { ...parsed };
                        this.updateHiddenFromSelection();
                    }
                }
            });
        }

        syncInitialValue() {
            if (this.hidden && this.hidden.value) {
                const parsed = parseGregorianValue(this.hidden.value);
                if (parsed) {
                    const persian = jdnToPersian(gregorianToJdn(parsed.year, parsed.month, parsed.day));
                    this.selectedDate = persian;
                    this.viewDate = { ...persian };
                    this.input.value = formatPersianDisplay(persian);
                    return;
                }
            }
            if (this.input.value) {
                const parsed = parsePersianValue(this.input.value);
                if (parsed) {
                    this.selectedDate = parsed;
                    this.viewDate = { ...parsed };
                    this.updateHiddenFromSelection();
                    this.input.value = formatPersianDisplay(parsed);
                    return;
                }
            }
            this.viewDate = getTodayPersian();
        }

        open() {
            if (this.visible) {
                this.render();
                this.positionPicker();
                return;
            }
            closeAll(this);
            this.render();
            this.picker.classList.add('is-open');
            this.picker.style.visibility = 'hidden';
            this.visible = true;
            this.positionPicker();
            this.picker.style.visibility = '';
            window.addEventListener('resize', this.boundReposition);
            window.addEventListener('scroll', this.boundReposition, true);
            this.focusActiveDay();
        }

        close() {
            if (!this.visible) {
                return;
            }
            this.picker.classList.remove('is-open');
            this.visible = false;
            window.removeEventListener('resize', this.boundReposition);
            window.removeEventListener('scroll', this.boundReposition, true);
        }

        clearSelection() {
            this.selectedDate = null;
            if (this.hidden) {
                this.hidden.value = '';
            }
        }

        changeMonth(offset) {
            let month = this.viewDate.month + offset;
            let year = this.viewDate.year;
            if (month < 1) {
                month = 12;
                year -= 1;
            } else if (month > 12) {
                month = 1;
                year += 1;
            }
            this.viewDate = { year, month, day: 1 };
            this.render();
            this.focusActiveDay();
        }

        updateHiddenFromSelection() {
            if (!this.selectedDate || !this.hidden) {
                return;
            }
            const jdn = persianToJdn(this.selectedDate.year, this.selectedDate.month, this.selectedDate.day);
            const gregorian = jdnToGregorian(jdn);
            this.hidden.value = formatGregorianHidden(gregorian);
        }

        updateInputFromSelection() {
            if (this.selectedDate) {
                this.input.value = formatPersianDisplay(this.selectedDate);
                this.updateHiddenFromSelection();
            } else {
                this.input.value = '';
                if (this.hidden) {
                    this.hidden.value = '';
                }
            }
        }

        selectDate(date) {
            this.selectedDate = { ...date };
            this.viewDate = { ...date };
            this.updateInputFromSelection();
            this.close();
        }

        focusActiveDay() {
            requestAnimationFrame(() => {
                const target = this.gridElement.querySelector('button.is-selected') ||
                    this.gridElement.querySelector('button.is-today:not(.is-muted)') ||
                    this.gridElement.querySelector('button:not(.is-muted)');
                if (target) {
                    target.focus();
                }
            });
        }

        render() {
            this.titleElement.textContent = `${MONTH_NAMES[this.viewDate.month - 1]} ${toPersianDigits(this.viewDate.year.toString())}`;
            this.gridElement.innerHTML = '';

            const currentMonthLength = persianMonthLength(this.viewDate.year, this.viewDate.month);
            const prevMonth = this.viewDate.month === 1
                ? { year: this.viewDate.year - 1, month: 12 }
                : { year: this.viewDate.year, month: this.viewDate.month - 1 };
            const nextMonth = this.viewDate.month === 12
                ? { year: this.viewDate.year + 1, month: 1 }
                : { year: this.viewDate.year, month: this.viewDate.month + 1 };
            const prevMonthLength = persianMonthLength(prevMonth.year, prevMonth.month);

            const firstDayGregorian = jdnToGregorian(persianToJdn(this.viewDate.year, this.viewDate.month, 1));
            const firstDayDate = new Date(Date.UTC(firstDayGregorian.year, firstDayGregorian.month - 1, firstDayGregorian.day));
            const leading = getPersianWeekIndex(firstDayDate.getUTCDay());

            const cells = [];
            for (let i = 0; i < leading; i++) {
                const day = prevMonthLength - leading + 1 + i;
                cells.push({ year: prevMonth.year, month: prevMonth.month, day, current: false });
            }
            for (let day = 1; day <= currentMonthLength; day++) {
                cells.push({ year: this.viewDate.year, month: this.viewDate.month, day, current: true });
            }
            const totalCells = Math.ceil(cells.length / 7) * 7;
            for (let i = 1; cells.length < totalCells; i++) {
                cells.push({ year: nextMonth.year, month: nextMonth.month, day: i, current: false });
            }

            const today = getTodayPersian();

            cells.forEach((cellDate) => {
                const wrapper = document.createElement('div');
                wrapper.className = 'persian-datepicker-day';
                const button = document.createElement('button');
                button.type = 'button';
                button.textContent = toPersianDigits(padNumber(cellDate.day, 2));
                button.setAttribute('aria-label', `${MONTH_NAMES[cellDate.month - 1]} ${toPersianDigits(cellDate.day.toString())}، ${toPersianDigits(cellDate.year.toString())}`);
                if (!cellDate.current) {
                    button.classList.add('is-muted');
                }
                if (datesEqual(cellDate, today)) {
                    button.classList.add('is-today');
                }
                if (this.selectedDate && datesEqual(cellDate, this.selectedDate)) {
                    button.classList.add('is-selected');
                }
                button.addEventListener('click', (event) => {
                    event.preventDefault();
                    this.selectDate(cellDate);
                });
                wrapper.appendChild(button);
                this.gridElement.appendChild(wrapper);
            });
        }

        positionPicker() {
            if (!this.visible) {
                return;
            }
            this.picker.style.width = '';
            const rect = this.input.getBoundingClientRect();
            const scrollX = window.pageXOffset;
            const scrollY = window.pageYOffset;
            const minWidth = Math.max(rect.width, 256);
            this.picker.style.width = `${minWidth}px`;
            const pickerRect = this.picker.getBoundingClientRect();
            let left = rect.right + scrollX - pickerRect.width;
            const viewportLeft = scrollX + 8;
            const viewportRight = scrollX + window.innerWidth - 8;
            if (left < viewportLeft) {
                left = viewportLeft;
            }
            if (left + pickerRect.width > viewportRight) {
                left = Math.max(viewportLeft, viewportRight - pickerRect.width);
            }
            let top = rect.bottom + scrollY + 8;
            if (top + pickerRect.height > scrollY + window.innerHeight - 8) {
                top = rect.top + scrollY - pickerRect.height - 8;
            }
            if (top < scrollY + 8) {
                top = rect.bottom + scrollY + 8;
            }
            this.picker.style.left = `${left}px`;
            this.picker.style.top = `${top}px`;
        }

        containsTarget(target) {
            return this.input.contains(target) || this.picker.contains(target);
        }
    }

    document.addEventListener('click', (event) => {
        instances.forEach((instance) => {
            if (instance.visible && !instance.containsTarget(event.target)) {
                instance.close();
            }
        });
    });

    document.addEventListener('keydown', (event) => {
        if (event.key === 'Escape') {
            instances.forEach((instance) => instance.close());
        }
    });

    window.PersianDatePicker = {
        init(selector) {
            const elements = selector
                ? document.querySelectorAll(selector)
                : document.querySelectorAll('[data-persian-picker]');
            elements.forEach((input) => {
                if (!input._persianDatePicker) {
                    input._persianDatePicker = new DatePicker(input);
                }
            });
        }
    };
})();
