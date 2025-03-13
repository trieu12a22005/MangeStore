const userRoutes = require("./user.route")
const systemConfig = require("../../../config/system")
const adminRoutes = require("./admin.route")
const employeeRoutes = require("./employee.route")
module.exports = (app) =>{
    const PATH_TASK = systemConfig.prefixTask
    app.use(PATH_TASK+"/users", userRoutes)
    app.use(PATH_TASK+"/admin", adminRoutes)
    app.use(PATH_TASK+"/employee", employeeRoutes)
}