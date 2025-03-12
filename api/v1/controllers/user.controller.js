const md5 = require("md5");
const User = require("../models/user.model");
const jwt = require("jsonwebtoken"); // thư viện để tạo token
const bcrypt = require("bcrypt"); // ✅ Import bcrypt đúng cách
const generateHelper = require("../../../helpers/generate");
const ForgotPassword = require("../models/forgot-pasword.model");
const sendMailHelper = require("../../../helpers/sendMail");
module.exports.register = async (req, res) => {
  req.body.password = md5(req.body.password);
  const existEmail = await User.findOne({
    email: req.body.email,
    deleted: false,
  });
  if (existEmail) {
    res.json({
      code: 400,
      message: "Email đã tồn lại",
    });
  } else {
    const user = new User({
      fullName: req.body.fullname,
      email: req.body.email,
      password: req.body.password,
      
    });
    await user.save();
    console.log(user);
    const token = jwt.sign({ userId: user._id }, process.env.SECRET_KEY, {
      expiresIn: "7d",
    });
    res.cookie("token", token, {
      httpOnly: true, // 🛡 Chống XSS (Không cho JavaScript đọc)
      secure: process.env.NODE_ENV === "development", // 🔐 Chỉ gửi cookie qua HTTPS nếu đang ở môi trường production
      sameSite: "Strict", // 🛑 Ngăn chặn CSRF
      maxAge: 7 * 24 * 60 * 60 * 1000, // ⏳ 7 ngày
    });
    res.json({
      code: 200,
      message: "Tạo tài khoản thành công",
      token: token,
    });
  }
};
module.exports.login = async (req, res) => {
  try {
    const { email, password } = req.body;

    // Kiểm tra user có tồn tại không
    const user = await User.findOne({ email, deleted: false });
    if (!user) {
      return res.status(400).json({ message: "Email không tồn tại!" });
    }

    // Kiểm tra mật khẩu
    if (md5(password) !== user.password) {
      res.json({
        code: 400,
        message: "sai mật khẩu",
      });
      return;
    }

    // Tạo JWT token mới
    const token = jwt.sign({ userId: user._id }, process.env.SECRET_KEY, {
      expiresIn: "7d",
    });

    // Lưu token vào HTTP-only Cookie
    res.cookie("token", token, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "development",
      sameSite: "Strict",
      maxAge: 7 * 24 * 60 * 60 * 1000,
    });

    res.status(200).json({ message: "Đăng nhập thành công!" });
  } catch (error) {
    res.status(500).json({ message: "Lỗi server", error: error.message });
  }
};
module.exports.forgotPassword = async (req, res) => {
  const email = req.body.email;
  const user = await User.findOne({
    email: email,
    deleted: false,
  });
  if (!user) {
    res.json({
      code: 400,
      message: "Email không tồn tại",
    });
    return;
  }
  const otp = generateHelper.generateRandomNumber(6);
  const objectForgotPassword = {
    email: email,
    otp: otp,
    expireAt: Date.now(),
  };
  const forgotPassword = new ForgotPassword(objectForgotPassword);
  await forgotPassword.save();
  // Gửi OTP qua email user
  const subject = "MÃ OTP xác minh lấy lại mật khẩu";
  const html = `Max OTP để lấy lại mật khẩu của bạn là <b>${otp}</b> (Sử dụng trong 3 phút).
                  Vui lòng không chia sẻ mã này với bất kì ai.
    `;
  sendMailHelper.sendMail(email, subject, html);
  res.json({
    code: 200,
    message: "Đã gửi mã OTP qua email!",
  });
};
module.exports.otpPassword = async (req, res) => {
  const email = req.body.email;
  const otp = req.body.otp;
  const result = await ForgotPassword.findOne({
    email: email,
    otp: otp,
  });
  if (!result) {
    res.json({
      code: 400,
      message: "Mã OTP không hợp lệ",
    });
    return;
  }
  const user = await User.findOne({
    email: email,
  });
  const token = jwt.sign({ userId: user._id }, process.env.SECRET_KEY, {
    expiresIn: "7d",
  });
  res.cookie("token", token, {
    httpOnly: true, // 🛡 Chống XSS (Không cho JavaScript đọc)
    secure: process.env.NODE_ENV === "development",
    sameSite: "Strict", // 🛑 Ngăn chặn CSRF
    maxAge: 7 * 24 * 60 * 60 * 1000, // ⏳ 7 ngày
  });
  res.json({
    code: 200,
  });
};
module.exports.resetPassword = async (req, res) => {
  const token = req.cookies.token;
  const password = req.body.password;
  // giải mã token
  const decoded = jwt.verify(token, process.env.SECRET_KEY);
  const userId = decoded.userId;
  const user = await User.findOne({
    _id: userId
  })
  if (!user){
    res.json({
        code: 400,
        message: "User không tồn tại"
    })
    return;
  }
  if (user.password === md5(password))
  {
    res.json({
        code: 400,
        message: "Mật khẩu mới không được trùng mật khẩu cũ"
    })
    return;
  }
  await User.updateOne(
    { _id: userId },   // Điều kiện để tìm đúng user
    { password: md5(password) } // Cập nhật mật khẩu
  );
  res.json({
    code: 200,
    message: "Cập nhật mật khẩu thành công"
  });
};
module.exports.detailUser = async(req,res) =>{

  const user= await User.findOne({
    _id: req.user.userId,
    deleted: false
  })
  console.log(user)
  res.json({
    code: 200
  })
}
module.exports.logout = async(req,res) =>{
  res.cookie("token", "", {
    httpOnly: true,
    secure: process.env.NODE_ENV === "development",
    sameSite: "Strict",
    expires: new Date(0), // ⏳ Ép hết hạn về quá khứ
    path: "/"
});
res.json({ 
  code: 200,
  message: "Đã logout và cookie đã bị xóa!" });

}