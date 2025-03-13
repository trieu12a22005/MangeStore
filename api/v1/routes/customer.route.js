const express = require("express")
const controllers = require("../controllers/customer.controller")
const verifyToken = require("../../../validate/verifyToken")
const router =  express.Router()
router.post("/reversation",verifyToken, controllers.postReversation);
router.get("/reversation",verifyToken,controllers.getReversation);
router.patch("/update/:id",verifyToken,controllers.updateTable)
router.delete("/delete/:id",verifyToken,controllers.deleteTable)
module.exports = router;
