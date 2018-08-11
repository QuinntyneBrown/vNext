"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var dashboard_settings_model_1 = require("./dashboard-settings.model");
var Dashboard = /** @class */ (function () {
    function Dashboard(code) {
        this.dashboardId = 0;
        this.settings = new dashboard_settings_model_1.DashboardSettings();
        this.dashboardTiles = [];
        this.code = code;
    }
    return Dashboard;
}());
exports.Dashboard = Dashboard;
//# sourceMappingURL=dashboard.model.js.map