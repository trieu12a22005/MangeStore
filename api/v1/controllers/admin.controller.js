const User = require("../models/user.model");
const shiftEmployee = require("../models/shiftEmployee.model");
const Shift = require("../models/shiftEmployee.model");
// thêm ca làm việc cho nhân viên
module.exports.shiftEmployee = async (req, res) => {
  const { email, fullName, startDate, startTime, endTime } = req.body;
  // Kiểm tra xem email có được gửi lên không
  if (!email || !fullName || !startDate || !startTime || !endTime) {
    return res
      .status(400)
      .json({ code: 400, message: "Thiếu thông tin bắt buộc" });
  }
  const user = await User.findOne({
    email: email,
    deleted: false,
  });
  if (!user)
    return res.json({
      code: 400,
      message: "Không có nhân viên này trong hệ thống",
    });
  const objectShifEmployee = {
    email: email,
    fullName: fullName,
    startDate: startDate,
    startTime: startTime,
    endTime: endTime,
  };
  const existingShift = await shiftEmployee.findOne({
    email: email,
    startDate: startDate,
    startTime: startTime,
    endTime: endTime,
  });

  if (existingShift) {
    return res.status(409).json({
      code: 409,
      message: "Ca làm việc này đã tồn tại, không thể thêm trùng!",
    });
  }
  const shift = new shiftEmployee(objectShifEmployee);
  await shift.save();
  res.json({
    code: 200,
    message: "Thêm ca làm việc thành công",
  });
};
// Xóa ca làm việc
module.exports.deleteShift = async (req, res) => {
  const { id } = req.params;
  if (!id) {
    return res.json({
      code: 400,
      message: "Thiếu id ca làm",
    });
  }
  const deleteShift = await shiftEmployee.findByIdAndDelete(id);
  if (!deleteShift) {
    return res.json({
      code: 400,
      message: "Không tìm thấy ca làm",
    });
  }
  console.log(deleteShift);
  res.json({
    code: 200,
  });
};
//update ca làm việc
module.exports.updateShift = async (req, res) => {
  const { id } = req.params;
  const updateData = req.body;
  if (!id) {
    return res.json({
      code: 400,
      message: "Thiếu id ca làm",
    });
  }
  const updatedShift = await Shift.findByIdAndUpdate(id, updateData, {
    new: true, // Trả về dữ liệu sau khi cập nhật
    runValidators: true, // Kiểm tra validation của schema
  });
  if (!updatedShift) {
    return res.status(404).json({
      code: 404,
      message: "Không tìm thấy ca làm để cập nhật",
    });
  }
  res.json({
    code: 200,
    message: "Cập nhật ca làm thành công",
    updatedShift,
  });
};
// Xóa tài khoản nhân viên
module.exports.deleteEmployee = async(req,res) =>{
  const { id } = req.params;
  if (!id) {
    return res.json({
      code: 400,
      message: "thiếu id nhân viên",
    })
  }
  const deleteEmployee = await User.findByIdAndDelete(id);
  if (!deleteEmployee)
  {
    return res.json({
      code: 400,
      message: "Không tìm thấy tài khoản nhân viên"
    })
  }
  res.json({
    code: 200,
    message: "Xóa thành công"
  })
}
