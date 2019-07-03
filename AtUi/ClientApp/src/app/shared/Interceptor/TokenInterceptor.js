"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TokenInterceptor = /** @class */ (function () {
    function TokenInterceptor() {
    }
    TokenInterceptor.prototype.intercept = function (httpReq, next) {
        var headers = httpReq.headers
            .set('Content-Type', 'application/json');
        if (headers.get('noToken') === 'noToken') {
            headers = headers.delete('Authorization').delete('noToken');
        }
        var newReq = httpReq.clone({ headers: headers });
        return next.handle(newReq);
    };
    return TokenInterceptor;
}());
exports.TokenInterceptor = TokenInterceptor;
//# sourceMappingURL=TokenInterceptor.js.map