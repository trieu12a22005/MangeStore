const express = require("express")
const router =  express.Router()
const controllers = require("../controllers/table.controller")
const verifyToken = require("../../../validate/verifyToken")
router.post("/add",verifyToken, controllers.addTable);
router.get("",verifyToken, controllers.getTable);
router.patch("/update/:id",verifyToken,controllers.updateTable)
router.delete("/delete/:id",verifyToken,controllers.deleteTable)
module.exports = router;