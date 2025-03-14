const mongoose = require("mongoose");

const ReservationSchema = new mongoose.Schema({
    customerName: {
        type: String,
        trim: true
    },
    phoneNumber: {
        type: String,
        trim: true
    },
    numberOfGuests: {
        type: Number,
        min: 1
    },
    reservationDate: {
        type: Date,
        
    },
    reservationTime: {
        type: String,
    },
    table: { type: mongoose.Schema.Types.ObjectId, ref: "Table" },
    status: {
        type: String,
        enum: ["Pending", "Confirmed", "Cancelled", "Completed"],
        default: "Pending"
    },
    createdAt: {
        type: Date,
        default: Date.now
    }
});
const Reservation = mongoose.model("Reservation", ReservationSchema,"Reservation");
module.exports = Reservation;
