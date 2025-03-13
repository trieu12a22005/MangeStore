const mongoose = require("mongoose")
const ShiftSchema = new mongoose.Schema(
    {
        fullName: {
            type: String, 
        },
        email: String,
        startDate: Date,
        startTime: String,
        endTime: String,
        completed: {
            type: Boolean,
            default: false
        }
    },
    {timestamps: true}
);
const Shift = mongoose.model("Shift", ShiftSchema, "shift");
module.exports= Shift