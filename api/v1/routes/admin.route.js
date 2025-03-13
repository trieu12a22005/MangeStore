const express = require("express")
const router =  express.Router()
const controllers = require("../controllers/admin.controller")
const verifyToken = require("../../../validate/verifyToken")
router.post("/shift",verifyToken, controllers.shiftEmployee);
router.delete("/delete/:id",verifyToken, controllers.deleteShift)
router.patch("/update/shift/:id",verifyToken,controllers.updateShift)
router.delete("/delete/employee/:id", verifyToken, controllers.deleteEmployee)
module.exports = router;