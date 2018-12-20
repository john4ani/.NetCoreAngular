import * as tslib_1 from "tslib";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Order, OrderItem } from '../shop/order'; //or import * as OrderNS from "/order"
var DataService = /** @class */ (function () {
    function DataService(http) {
        this.http = http;
        this.token = "";
        this.order = new Order();
        this.products = [];
    }
    DataService.prototype.loadProducts = function () {
        var _this = this;
        return this.http.get("/api/products")
            .pipe(map(function (data) {
            _this.products = data;
            return true;
        }));
    };
    Object.defineProperty(DataService.prototype, "loginRequired", {
        get: function () {
            return this.token.length == 0 || this.tokenExpiration > new Date();
        },
        enumerable: true,
        configurable: true
    });
    DataService.prototype.login = function (creds) {
        var _this = this;
        return this.http.post("/account/createtoken", creds)
            .pipe(map(function (data) {
            _this.token = data.token;
            _this.tokenExpiration = data.expiration;
            return true;
        }));
    };
    DataService.prototype.checkout = function () {
        var _this = this;
        if (!this.order.orderNumber)
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        return this.http.post("/api/orders", this.order, {
            headers: new HttpHeaders({ "Authorization": "Bearer " + this.token })
        })
            .pipe(map(function (response) {
            _this.order = new Order();
            return true;
        }));
    };
    DataService.prototype.addOrder = function (newProduct) {
        var item = this.order.items.find(function (i) { return i.productId == newProduct.id; });
        if (item) {
            item.quantity++;
        }
        else {
            item = new OrderItem();
            item.productId = newProduct.id;
            item.productArtist = newProduct.artist;
            item.productTitle = newProduct.title;
            item.unitPrice = newProduct.price;
            item.productCategory = newProduct.category;
            item.quantity = 1;
            this.order.items.push(item);
        }
    };
    DataService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpClient])
    ], DataService);
    return DataService;
}());
export { DataService };
//# sourceMappingURL=dataService.js.map