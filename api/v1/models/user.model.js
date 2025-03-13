const mongoose = require("mongoose")
const userSchema = new mongoose.Schema(
    {
        fullName: {
            type: String, 
        },
        email: String,
        password: String,
        birth: String,
        role: {
            type: String,
            default: "employee"
        },
        deleted: {
            type: Boolean,
            default: false
        },
        deletedAt: Date,
    },
    {timestamps: true}
);
const User = mongoose.model("User", userSchema, "users");
module.exports= User