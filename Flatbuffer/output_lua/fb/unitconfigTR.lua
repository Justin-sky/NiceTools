-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: fb

local flatbuffers = require('flatbuffers')

local unitconfigTR = {} -- the module
local unitconfigTR_mt = {} -- the class metatable

function unitconfigTR.New()
    local o = {}
    setmetatable(o, {__index = unitconfigTR_mt})
    return o
end
function unitconfigTR.GetRootAsunitconfigTR(buf, offset)
    local n = flatbuffers.N.UOffsetT:Unpack(buf, offset)
    local o = unitconfigTR.New()
    o:Init(buf, n + offset)
    return o
end
function unitconfigTR_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function unitconfigTR_mt:_id()
    local o = self.view:Offset(4)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function unitconfigTR_mt:_name()
    local o = self.view:Offset(6)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function unitconfigTR_mt:_desc()
    local o = self.view:Offset(8)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function unitconfigTR_mt:_position()
    local o = self.view:Offset(10)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function unitconfigTR_mt:_height()
    local o = self.view:Offset(12)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function unitconfigTR_mt:_weight()
    local o = self.view:Offset(14)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function unitconfigTR.Start(builder) builder:StartObject(6) end
function unitconfigTR.Add_id(builder, Id) builder:PrependInt32Slot(0, Id, 0) end
function unitconfigTR.Add_name(builder, Name) builder:PrependUOffsetTRelativeSlot(1, Name, 0) end
function unitconfigTR.Add_desc(builder, Desc) builder:PrependUOffsetTRelativeSlot(2, Desc, 0) end
function unitconfigTR.Add_position(builder, Position) builder:PrependInt32Slot(3, Position, 0) end
function unitconfigTR.Add_height(builder, Height) builder:PrependInt32Slot(4, Height, 0) end
function unitconfigTR.Add_weight(builder, Weight) builder:PrependInt32Slot(5, Weight, 0) end
function unitconfigTR.End(builder) return builder:EndObject() end

return unitconfigTR -- return the module