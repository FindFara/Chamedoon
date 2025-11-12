(function (global) {
    const documentRef = global.document;
    if (!documentRef) {
        return;
    }

    const breaks = [-61, 9, 38, 199, 426, 686, 756, 818, 1111, 1181, 1210, 1635, 2060, 2097, 2192, 2262, 2324, 2394, 2456, 3178];

    const div = (a, b) => Math.floor(a / b);
    const mod = (a, b) => a - Math.floor(a / b) * b;

    const g2d = (gy, gm, gd) => {
        let d = div((gy + div(gm - 8, 6) + 100100) * 1461, 4);
        d += div(153 * mod(gm + 9, 12) + 2, 5);
        d += gd - 34840408;
        d -= div(div(gy + 100100 + div(gm - 8, 6), 100) * 3, 4);
        return d + 752;
    };

    const d2g = (jdn) => {
        let j = 4 * jdn + 139361631;
        j += div(div(4 * jdn + 183187720, 146097) * 3, 4) * 4 - 3908;
        const i = div(mod(j, 1461), 4) * 5 + 308;
        const gd = div(mod(i, 153), 5) + 1;
        const gm = mod(div(i, 153), 12) + 1;
        const gy = div(j, 1461) - 100100 + div(8 - gm, 6);
        return { gy, gm, gd };
    };

    const jalCal = (jy) => {
        let bl = breaks.length;
        let gy = jy + 621;
        let leapJ = -14;
        let jp = breaks[0];
        let jm = 0;
        let jump = 0;

        if (jy < jp || jy >= breaks[bl - 1]) {
            throw new RangeError('Invalid Jalali year');
        }

        for (let i = 1; i < bl; i += 1) {
            jm = breaks[i];
            jump = jm - jp;
            if (jy < jm) {
                break;
            }
            leapJ += div(jump, 33) * 8 + div(mod(jump, 33), 4);
            jp = jm;
        }

        let n = jy - jp;
        leapJ += div(n, 33) * 8 + div(mod(n, 33) + 3, 4);
        if (mod(jump, 33) === 4 && jump - n === 4) {
            leapJ += 1;
        }

        const leapG = div(gy, 4) - div((div(gy, 100) + 1) * 3, 4) - 150;
        const march = 20 + leapJ - leapG;

        if (jump - n < 6) {
            n = n - jump + div(jump + 4, 33) * 33;
        }
        let leap = mod(n + 1, 33) - 1;
        if (leap === -1) {
            leap = 4;
        }
        return { leap, gy, march };
    };

    const j2d = (jy, jm, jd) => {
        const r = jalCal(jy);
        return g2d(r.gy, 3, r.march) + (jm - 1) * 31 - div(jm, 7) * (jm - 7) + jd - 1;
    };

    const d2j = (jdn) => {
        const g = d2g(jdn);
        let jy = g.gy - 621;
        const r = jalCal(jy);
        const jdn1f = g2d(g.gy, 3, r.march);
        let k = jdn - jdn1f;
        let jm;
        let jd;

        if (k >= 0) {
            if (k <= 185) {
                jm = 1 + div(k, 31);
                jd = mod(k, 31) + 1;
                return { jy, jm, jd };
            }
            k -= 186;
        } else {
            jy -= 1;
            k += 179;
            if (r.leap === 1) {
                k += 1;
            }
        }

        jm = 7 + div(k, 30);
        jd = mod(k, 30) + 1;
        return { jy, jm, jd };
    };

    const toGregorian = (jy, jm, jd) => {
        const result = d2g(j2d(jy, jm, jd));
        return [result.gy, result.gm, result.gd];
    };

    const toJalali = (gy, gm, gd) => {
        const result = d2j(g2d(gy, gm, gd));
        return [result.jy, result.jm, result.jd];
    };

    const pad = (value) => value.toString().padStart(2, '0');

    const toIsoString = (parts) => `${parts[0]}-${pad(parts[1])}-${pad(parts[2])}`;
    const toJalaliString = (parts) => `${parts[0]}/${pad(parts[1])}/${pad(parts[2])}`;

    const isoToJalali = (isoValue) => {
        if (!isoValue) {
            return '';
        }
        const [datePart] = isoValue.split('T');
        const [year, month, day] = datePart.split('-').map((part) => parseInt(part, 10));
        if ([year, month, day].some((value) => Number.isNaN(value))) {
            return '';
        }
        try {
            const jalaliParts = toJalali(year, month, day);
            return toJalaliString(jalaliParts);
        } catch (error) {
            return '';
        }
    };

    const jalaliToIso = (jalaliValue) => {
        if (!jalaliValue) {
            return '';
        }
        const cleaned = jalaliValue.replace(/\s+/g, '');
        const parts = cleaned.split('/').map((part) => parseInt(part, 10));
        if (parts.length !== 3 || parts.some((value) => Number.isNaN(value))) {
            return '';
        }
        const [jy, jm, jd] = parts;
        try {
            const gregorian = toGregorian(jy, jm, jd);
            return toIsoString(gregorian);
        } catch (error) {
            return '';
        }
    };

    const getAltField = (input) => {
        const selector = input.dataset.altField;
        if (!selector) {
            return null;
        }
        return documentRef.querySelector(selector);
    };

    const applyInitialValues = (input) => {
        const altField = getAltField(input);
        const initialIso = input.dataset.initialValue || (altField ? altField.value : '');
        if (initialIso) {
            const jalaliValue = isoToJalali(initialIso);
            if (jalaliValue) {
                input.value = jalaliValue;
            }
        }
    };

    const updateAltField = (input) => {
        const altField = getAltField(input);
        if (!altField) {
            return;
        }
        const isoValue = jalaliToIso(input.value);
        altField.value = isoValue;
    };

    const clearInput = (input) => {
        input.value = '';
        const altField = getAltField(input);
        if (altField) {
            altField.value = '';
        }
    };

    const attachLibraryFlag = (input) => {
        input.setAttribute('data-jdp', '');
        input.setAttribute('data-jdp-only-date', 'true');
    };

    const bindInputEvents = (input) => {
        input.addEventListener('change', () => updateAltField(input));
        input.addEventListener('blur', () => updateAltField(input));
    };

    const initialise = () => {
        const inputs = Array.from(documentRef.querySelectorAll('.jalali-date-input'));
        inputs.forEach((input) => {
            applyInitialValues(input);
            attachLibraryFlag(input);
            bindInputEvents(input);
        });

        if (global.jalaliDatepicker && typeof global.jalaliDatepicker.startWatch === 'function') {
            global.jalaliDatepicker.startWatch({
                time: false,
                date: {
                    format: 'YYYY/MM/DD',
                },
            });
        }

        documentRef.querySelectorAll('.jalali-input-trigger').forEach((trigger) => {
            trigger.addEventListener('click', (event) => {
                event.preventDefault();
                const targetSelector = trigger.dataset.target;
                if (!targetSelector) {
                    return;
                }
                const input = documentRef.querySelector(targetSelector);
                if (input) {
                    input.focus();
                    if (global.jalaliDatepicker && typeof global.jalaliDatepicker.show === 'function') {
                        try {
                            global.jalaliDatepicker.show(input);
                        } catch (error) {
                            // ignore show errors
                        }
                    }
                }
            });
        });

        documentRef.querySelectorAll('.jalali-clear-button').forEach((button) => {
            button.addEventListener('click', (event) => {
                event.preventDefault();
                const targetSelector = button.dataset.target;
                if (!targetSelector) {
                    return;
                }
                const input = documentRef.querySelector(targetSelector);
                if (input) {
                    clearInput(input);
                }
            });
        });
    };

    if (documentRef.readyState === 'loading') {
        documentRef.addEventListener('DOMContentLoaded', initialise);
    } else {
        initialise();
    }
})(window);
