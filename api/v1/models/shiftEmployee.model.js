const mongoose = require("mongoose")
const ShiftSchema = new mongoose.Schema(
    {
        fullName: {
            type: String, 
        },
        email: String,
        startDate: Date,
        startTime: String,
        endTime: String
    },
    {timestamps: true}
);
const Shift = mongoose.model("Shift", ShiftSchema, "shift");
module.exports= Shift