const express = require("express")
const router =  express.Router()
const controllers = require("../controllers/employee.controller")
const verifyToken = require("../../../validate/verifyToken")
router.get("/detail",verifyToken, controllers.getShiftEmployee);
module.exports = router;