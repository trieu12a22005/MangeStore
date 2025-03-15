const Table = require("../models/table.model")
module.exports.addTable = async(req,res) =>{
    const {tableNumber,capacity, available} = req.body;
    if (!tableNumber || !capacity)
    {
        return res.json({
            code: 400,
            message: "Chưa điền đủ thông tin hoặc sai thông tin"
        })
    }
    const findTable = await Table.findOne({
        tableNumber: tableNumber
    })
    if (findTable)
    {
       return res.json({
            code: 400,
            message:"Bàn này đã được thêm rồi"
        })
    }
    const newTable = new Table({
       tableNumber,
       capacity,
       available 
    })
    const saveTable = await newTable.save();
    res.status(201).json(saveTable);
}
module.exports.getTable = async(req,res) =>{
    const listTable = await Table.find();
    res.json(listTable)
}
module.exports.updateTable = async (req,res) =>{
    const { id } = req.params;
      const updateData = req.body;
      if (!id) {
        return res.json({
          code: 400,
          message: "Thiếu id bàn",
        });
      }
      const updatedTable = await Table.findByIdAndUpdate(id, updateData, {
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
module.exports.deleteTable = async (req,res) =>{
    const {id} =  req.params;
    if (!id) {
        return res.json({
          code: 400,
          message: "Thiếu id bàn",
        });
      }
    const deleteTable = await Table.findOneAndDelete({
        _id: id,
        available: true
    })
    if (!deleteTable) {
        return res.status(404).json({ message: "Không tìm thấy bàn hoặc bàn không khả dụng để xóa" });
    }
    res.json({
        code: 200,
        message: "xóa bàn thành công"
    })
}