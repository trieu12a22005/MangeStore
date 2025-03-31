const express = require("express")
const controllers = require("../controllers/customer.controller")
const verifyToken = require("../../../validate/verifyToken")
const router =  express.Router()
router.post("/reservation",verifyToken, controllers.postReversation);
router.get("",verifyToken,controllers.getReversation);
router.patch("/update/:id",verifyToken,controllers.updateTable)
router.delete("/delete/:id",verifyToken,controllers.deleteTable)
module.exports = router;
