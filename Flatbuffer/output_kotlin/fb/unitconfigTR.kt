// automatically generated by the FlatBuffers compiler, do not modify

package fb

import java.nio.*
import kotlin.math.sign
import com.google.flatbuffers.*

@Suppress("unused")
@ExperimentalUnsignedTypes
class unitconfigTR : Table() {

    fun __init(_i: Int, _bb: ByteBuffer)  {
        __reset(_i, _bb)
    }
    fun __assign(_i: Int, _bb: ByteBuffer) : unitconfigTR {
        __init(_i, _bb)
        return this
    }
    val Id : Int
        get() {
            val o = __offset(4)
            return if(o != 0) bb.getInt(o + bb_pos) else 0
        }
    val Name : String?
        get() {
            val o = __offset(6)
            return if (o != 0) __string(o + bb_pos) else null
        }
    val NameAsByteBuffer : ByteBuffer get() = __vector_as_bytebuffer(6, 1)
    fun NameInByteBuffer(_bb: ByteBuffer) : ByteBuffer = __vector_in_bytebuffer(_bb, 6, 1)
    val Desc : String?
        get() {
            val o = __offset(8)
            return if (o != 0) __string(o + bb_pos) else null
        }
    val DescAsByteBuffer : ByteBuffer get() = __vector_as_bytebuffer(8, 1)
    fun DescInByteBuffer(_bb: ByteBuffer) : ByteBuffer = __vector_in_bytebuffer(_bb, 8, 1)
    val Position : Int
        get() {
            val o = __offset(10)
            return if(o != 0) bb.getInt(o + bb_pos) else 0
        }
    val Height : Int
        get() {
            val o = __offset(12)
            return if(o != 0) bb.getInt(o + bb_pos) else 0
        }
    val Weight : Int
        get() {
            val o = __offset(14)
            return if(o != 0) bb.getInt(o + bb_pos) else 0
        }
    override fun keysCompare(o1: Int, o2: Int, _bb: ByteBuffer) : Int {
        val val_1 = _bb.getInt(__offset(4, o1, _bb))
        val val_2 = _bb.getInt(__offset(4, o2, _bb))
        return (val_1 - val_2).sign
    }
    companion object {
        fun validateVersion() = Constants.FLATBUFFERS_1_12_0()
        fun getRootAsunitconfigTR(_bb: ByteBuffer): unitconfigTR = getRootAsunitconfigTR(_bb, unitconfigTR())
        fun getRootAsunitconfigTR(_bb: ByteBuffer, obj: unitconfigTR): unitconfigTR {
            _bb.order(ByteOrder.LITTLE_ENDIAN)
            return (obj.__assign(_bb.getInt(_bb.position()) + _bb.position(), _bb))
        }
        fun createunitconfigTR(builder: FlatBufferBuilder, Id: Int, NameOffset: Int, DescOffset: Int, Position: Int, Height: Int, Weight: Int) : Int {
            builder.startTable(6)
            addWeight(builder, Weight)
            addHeight(builder, Height)
            addPosition(builder, Position)
            addDesc(builder, DescOffset)
            addName(builder, NameOffset)
            addId(builder, Id)
            return endunitconfigTR(builder)
        }
        fun startunitconfigTR(builder: FlatBufferBuilder) = builder.startTable(6)
        fun addId(builder: FlatBufferBuilder, Id: Int) = builder.addInt(0, Id, 0)
        fun addName(builder: FlatBufferBuilder, Name: Int) = builder.addOffset(1, Name, 0)
        fun addDesc(builder: FlatBufferBuilder, Desc: Int) = builder.addOffset(2, Desc, 0)
        fun addPosition(builder: FlatBufferBuilder, Position: Int) = builder.addInt(3, Position, 0)
        fun addHeight(builder: FlatBufferBuilder, Height: Int) = builder.addInt(4, Height, 0)
        fun addWeight(builder: FlatBufferBuilder, Weight: Int) = builder.addInt(5, Weight, 0)
        fun endunitconfigTR(builder: FlatBufferBuilder) : Int {
            val o = builder.endTable()
            return o
        }
        fun __lookup_by_key(obj: unitconfigTR?, vectorLocation: Int, key: Int, bb: ByteBuffer) : unitconfigTR? {
            var span = bb.getInt(vectorLocation - 4)
            var start = 0
            while (span != 0) {
                var middle = span / 2
                val tableOffset = __indirect(vectorLocation + 4 * (start + middle), bb)
                val value = bb.getInt(__offset(4, bb.capacity() - tableOffset, bb))
                val comp = value.compareTo(key)
                when {
                    comp > 0 -> span = middle
                    comp < 0 -> {
                        middle++
                        start += middle
                        span -= middle
                    }
                    else -> {
                        return (obj ?: unitconfigTR()).__assign(tableOffset, bb)
                    }
                }
            }
            return null
        }
    }
}
