const customer = require("../models/customer.model");
const tableModel = require("../models/table.model");

module.exports.postReversation = async (req, res) => {
    try {
        const { customerName, phoneNumber, numberOfGuests, reservationDate, reservationTime, tableNumber } = req.body;

        // Kiểm tra bàn có sẵn không
        const Table = await tableModel.findOne({ tableNumber, available: true });
        if (!Table) {
            return res.status(400).json({ message: "Bàn đã được đặt hoặc không tồn tại" });
        }

        // Kiểm tra số khách hợp lệ
        if (numberOfGuests < 1) {
            return res.status(400).json({ message: "Number of guests must be at least 1" });
        }

        // Kiểm tra định dạng ngày
        const date = new Date(reservationDate);
        if (isNaN(date.getTime())) {
            return res.status(400).json({ message: "Invalid reservation date" });
        }

        // Kiểm tra nếu bàn đã được đặt
        const existingReservation = await customer.findOne({ table: Table._id });
        if (existingReservation) {
            return res.status(400).json({ message: "Bàn đã được đặt" });
        }

        // Tạo đặt bàn mới
        const newReservation = new customer({
            customerName,
            phoneNumber,
            numberOfGuests,
            reservationDate: date,
            reservationTime,
            table: Table._id,
        });

        // Lưu vào database
        await newReservation.save();

        // Cập nhật trạng thái bàn thành "đã được đặt"
        Table.available = false;
        await Table.save();

        res.status(201).json({ message: "Reservation successful", reservation: newReservation });

    } catch (error) {
        console.error("❌ Error making reservation:", error);
        res.status(500).json({ message: "Internal Server Error", error: error.message });
    }
};

module.exports.getReversation = async (req,res) =>{
    const reversation = await customer.find().populate("table", "tableNumber");
    res.json(reversation)
}
module.exports.updateTable = async(req,res) =>{
    const { id } = req.params;
      const updateData = req.body;
      if (!id) {
        return res.json({
          code: 400,
          message: "Thiếu id bàn",
        });
      }
      const updatedTable = await customer.findByIdAndUpdate(id, updateData, {
        new: true, // Trả về dữ liệu sau khi cập nhật
        runValidators: true, // Kiểm tra validation của schema
      });
      if (!updatedTable) {
        return res.status(404).json({
          code: 404,
          message: "Không tìm thấy bàn để cập nhật",
        });
      }
      res.json({
        code: 200,
        message: "Cập nhật bàn thành công",
        updatedTable,
      });
}
//deleted
module.exports.deleteTable = async(req,res) =>{
    const { id } = req.params;
    const deletedTable = await customer.findByIdAndDelete(id);
    if(!deletedTable)
    {
      return  res.json({
            code: 400,
            message: "Không tìm thấy bàn để xóa"
        })
    }
    res.json({
        code:200,
        message: "Đã xóa thành công"
    })
}