"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var base_overlay_service_1 = require("../core/base-overlay.service");
var add_dashboard_tile_overlay_component_1 = require("./add-dashboard-tile-overlay.component");
var AddDashboardTileOverlay = /** @class */ (function (_super) {
    __extends(AddDashboardTileOverlay, _super);
    function AddDashboardTileOverlay(injector, overlayRefProvider) {
        return _super.call(this, injector, overlayRefProvider, add_dashboard_tile_overlay_component_1.AddDashboardTileOverlayComponent) || this;
    }
    return AddDashboardTileOverlay;
}(base_overlay_service_1.BaseOverlayService));
exports.AddDashboardTileOverlay = AddDashboardTileOverlay;
//# sourceMappingURL=add-dashboard-tile-overlay.js.map