define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function noVat(value) {
        var v = value;
        if (/^NO[0-9]{9}$/.test(v)) {
            v = v.substr(2);
        }
        if (!/^[0-9]{9}$/.test(v)) {
            return {
                meta: {},
                valid: false,
            };
        }
        var weight = [3, 2, 7, 6, 5, 4, 3, 2];
        var sum = 0;
        for (var i = 0; i < 8; i++) {
            sum += parseInt(v.charAt(i), 10) * weight[i];
        }
        sum = 11 - (sum % 11);
        if (sum === 11) {
            sum = 0;
        }
        return {
            meta: {},
            valid: "".concat(sum) === v.substr(8, 1),
        };
    }
    exports.default = noVat;
});
