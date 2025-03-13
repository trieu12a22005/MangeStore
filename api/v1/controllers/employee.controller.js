const User = require("../models/user.model");
const shiftEmployee = require("../models/shiftEmployee.model")
module.exports.getShiftEmployee =  async (req,res) =>{
    const user= await User.findOne({
      _id: req.user.userId,
      deleted: false
    })
    const shift = await shiftEmployee.find({
      email: user.email
    }).select("-createdAt -updatedAt -__v")
    if (!shift)
      return res.json({
    code: 400,
    message: "Nhân viên chưa có ca làm"
    })
    res.json({
      shift
    })
  }