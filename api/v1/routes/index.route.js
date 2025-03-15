const userRoutes = require("./user.route")
const systemConfig = require("../../../config/system")
const adminRoutes = require("./admin.route")
const employeeRoutes = require("./employee.route")
const menu = require("./menu.route")
const customer = require("./customer.route")
const table = require("./table.route")
module.exports = (app) =>{
    const PATH_TASK = systemConfig.prefixTask
    app.use(PATH_TASK+"/users", userRoutes)
    app.use(PATH_TASK+"/admin", adminRoutes)
    app.use(PATH_TASK+"/employee", employeeRoutes)
    app.use(PATH_TASK+"/menu",menu)
    app.use(PATH_TASK+"/customer",customer)
    app.use(PATH_TASK+"/table",table)
}