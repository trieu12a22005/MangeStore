const mongoose = require("mongoose")
const userSchema = new mongoose.Schema(
    {
        fullName: {
            type: String, 
        },
        email: String,
        password: String,
        token:{
            type: String,
        },
        role: {
            type: String
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