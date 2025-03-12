const userRoutes = require("./user.route")
const systemConfig = require("../../../config/system")
module.exports = (app) =>{
    const PATH_TASK = systemConfig.prefixTask
    app.use(PATH_TASK+"/users", userRoutes)
}