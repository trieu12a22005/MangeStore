const express = require("express")
const router =  express.Router()
const controllers = require("../controllers/admin.controller")
const verifyToken = require("../../../validate/verifyToken")
const checkAdmin = require("../../../validate/checkAdmin")
router.post("/shift",verifyToken, controllers.shiftEmployee);
router.delete("/delete/:id",checkAdmin, controllers.deleteShift)
router.patch("/update/shift/:id",verifyToken,controllers.updateShift)
router.delete("/delete/employee/:id", verifyToken, controllers.deleteEmployee)
module.exports = router;