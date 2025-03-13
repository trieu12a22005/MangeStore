const mongoose = require("mongoose");

const MenuSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true, // Bắt buộc phải có tên món ăn
        trim: true
    },
    description: {
        type: String,
        required: false, // Mô tả có thể có hoặc không
        trim: true
    },
    price: {
        type: Number,
        required: true, // Giá bắt buộc phải có
        min: 0
    },
    category: {
        type: String,
        required: true, // Loại món ăn (Ví dụ: Đồ ăn, Đồ uống, Tráng miệng)
        enum: ["Food", "Drink", "Dessert"], // Chỉ cho phép một số giá trị
        trim: true
    },
    imageUrl: {
        type: String, // URL hình ảnh món ăn
        required: false
    },
    available: {
        type: Boolean,
        default: true // Mặc định là có sẵn
    },
    ingredients: {
        type: [String], // Danh sách nguyên liệu
        required: false
    },
    discount: {
        type: Number, // Số tiền hoặc phần trăm giảm giá
        required: false,
        default: 0
    },
    createdAt: {
        type: Date,
        default: Date.now // Tự động lấy thời gian hiện tại
    }
},
{timestamps: true});

const Menu = mongoose.model("Menu", MenuSchema, "menu");
module.exports = Menu;
