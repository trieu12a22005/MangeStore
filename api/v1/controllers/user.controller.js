const md5 = require("md5");
const User = require("../models/user.model");
const jwt = require("jsonwebtoken");
const generateHelper = require("../../../helpers/generate");
const ForgotPassword = require("../models/forgot-pasword.model");
const shiftEmployee = require("../models/shiftEmployee.model");
const sendMailHelper = require("../../../helpers/sendMail");
module.exports.register = async (req, res) => {
  req.body.password = md5(req.body.password);
  const existEmail = await User.findOne({ email: req.body.email, deleted: false });
  if (existEmail) {
    return res.json({ code: 400, message: "Email đã tồn tại" });
  }

  const user = new User({
    fullName: req.body.fullName,
    email: req.body.email,
    birth: req.body.birth,
    role: req.body.role,
    password: req.body.password,
  });

  await user.save();

  const token = jwt.sign({ userId: user._id }, process.env.SECRET_KEY, { expiresIn: "7d" });

  res.json({
    code: 200,
    message: "Tạo tài khoản thành công",
    token: token, // ✅ FE sẽ lưu token và gửi qua header
  });
};

module.exports.login = async (req, res) => {
  const { email, password } = req.body;

  const user = await User.findOne({ email, deleted: false });
  if (!user) {
    return res.status(400).json({ message: "Email không tồn tại!" });
  }

  if (md5(password) !== user.password) {
    return res.json({ code: 400, message: "Sai mật khẩu" });
  }

  const token = jwt.sign({ userId: user._id }, process.env.SECRET_KEY, { expiresIn: "7d" });

  res.json({ code: 200, message: "Đăng nhập thành công!", token: token });
};

module.exports.forgotPassword = async (req, res) => {
  const email = req.body.email;
  const user = await User.findOne({ email, deleted: false });

  if (!user) {
    return res.json({ code: 400, message: "Email không tồn tại" });
  }

  const otp = generateHelper.generateRandomNumber(6);
  const forgotPassword = new ForgotPassword({ email, otp, expireAt: Date.now() });
  await forgotPassword.save();

  const subject = "MÃ OTP xác minh lấy lại mật khẩu";
  const html = `Mã OTP để lấy lại mật khẩu của bạn là <b>${otp}</b> (Sử dụng trong 3 phút).`;
  sendMailHelper.sendMail(email, subject, html);

  res.json({ code: 200, message: "Đã gửi mã OTP qua email!" });
};

module.exports.otpPassword = async (req, res) => {
  const { email, otp } = req.body;

  const result = await ForgotPassword.findOne({ email, otp });
  if (!result) {
    return res.json({ code: 400, message: "Mã OTP không hợp lệ" });
  }

  const user = await User.findOne({ email });
  const token = jwt.sign({ userId: user._id }, process.env.SECRET_KEY, { expiresIn: "7d" });

  res.json({ code: 200, token: token }); // ✅ FE sẽ nhận token mới
};

module.exports.resetPassword = async (req, res) => {
  const userId = req.user.userId;
  const { password } = req.body;

  const user = await User.findById(userId);
  if (!user) {
    return res.json({ code: 400, message: "User không tồn tại" });
  }

  if (user.password === md5(password)) {
    return res.json({ code: 400, message: "Mật khẩu mới không được trùng mật khẩu cũ" });
  }

  await User.updateOne({ _id: userId }, { password: md5(password) });

  res.json({ code: 200, message: "Cập nhật mật khẩu thành công" });
};

module.exports.detailUser = async (req, res) => {
  const user = await User.findOne({ _id: req.user.userId, deleted: false });
  if (!user) {
    return res.status(404).json({ message: "Không tìm thấy người dùng" });
  }

  res.json({
    fullName: user.fullName,
    email: user.email,
    birth: user.birth,
    role: user.role,
  });
};

module.exports.logout = async (req, res) => {
  // FE tự xóa token, BE không lưu token → không cần xoá gì
  res.json({
    code: 200,
    message: "FE cần xoá token ở localStorage/cookie",
  });
};
