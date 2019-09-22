"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/common/http");
var AppService = /** @class */ (function () {
    function AppService(http) {
        this.http = http;
        this.baseUri = 'api/Calculation';
    }
    AppService.prototype.sum = function (input) {
        var requestHeader = { headers: new http_1.HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'False' }) };
        return this.http.post(this.baseUri, input, requestHeader);
    };
    return AppService;
}());
exports.AppService = AppService;
//# sourceMappingURL=app-service.js.map