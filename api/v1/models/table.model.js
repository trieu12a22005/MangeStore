const mongoose = require("mongoose")
const TableSchema = new mongoose.Schema(
    {
        tableNumber: String,
        capacity: Number,
        available: {
            type: Boolean,
            default: true
        }
    },
    {timestamps: true}
);
const Table = mongoose.model("Table",TableSchema, "table");
module.exports= Table