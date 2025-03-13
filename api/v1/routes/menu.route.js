const express = require("express")
const controllers = require("../controllers/menu.controller")
const verifyToken = require("../../../validate/verifyToken")
const router =  express.Router()
router.post("/food",verifyToken, controllers.postFood);
router.get("",verifyToken,controllers.menu);
router.delete("/delete/:id",verifyToken, controllers.deleteFood)
router.patch("/update/:id",verifyToken, controllers.updateFood)
module.exports = router;
