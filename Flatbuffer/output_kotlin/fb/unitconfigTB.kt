// automatically generated by the FlatBuffers compiler, do not modify

package fb

import java.nio.*
import kotlin.math.sign
import com.google.flatbuffers.*

@Suppress("unused")
@ExperimentalUnsignedTypes
class unitconfigTB : Table() {

    fun __init(_i: Int, _bb: ByteBuffer)  {
        __reset(_i, _bb)
    }
    fun __assign(_i: Int, _bb: ByteBuffer) : unitconfigTB {
        __init(_i, _bb)
        return this
    }
    fun unitconfigTRS(j: Int) : fb.unitconfigTR? = unitconfigTRS(fb.unitconfigTR(), j)
    fun unitconfigTRS(obj: fb.unitconfigTR, j: Int) : fb.unitconfigTR? {
        val o = __offset(4)
        return if (o != 0) {
            obj.__assign(__indirect(__vector(o) + j * 4), bb)
        } else {
            null
        }
    }
    val unitconfigTRSLength : Int
        get() {
            val o = __offset(4); return if (o != 0) __vector_len(o) else 0
        }
    fun unitconfigTRSByKey(key: Int) : fb.unitconfigTR? {
        val o = __offset(4)
        return if (o != 0) {
            fb.unitconfigTR.__lookup_by_key(null, __vector(o), key, bb)
        } else {
            null
        }
    }
    fun unitconfigTRSByKey(obj: fb.unitconfigTR, key: Int) : fb.unitconfigTR? {
        val o = __offset(4)
        return if (o != 0) {
            fb.unitconfigTR.__lookup_by_key(obj, __vector(o), key, bb)
        } else {
            null
        }
    }
    companion object {
        fun validateVersion() = Constants.FLATBUFFERS_1_12_0()
        fun getRootAsunitconfigTB(_bb: ByteBuffer): unitconfigTB = getRootAsunitconfigTB(_bb, unitconfigTB())
        fun getRootAsunitconfigTB(_bb: ByteBuffer, obj: unitconfigTB): unitconfigTB {
            _bb.order(ByteOrder.LITTLE_ENDIAN)
            return (obj.__assign(_bb.getInt(_bb.position()) + _bb.position(), _bb))
        }
        fun createunitconfigTB(builder: FlatBufferBuilder, unitconfigTRSOffset: Int) : Int {
            builder.startTable(1)
            addUnitconfigTRS(builder, unitconfigTRSOffset)
            return endunitconfigTB(builder)
        }
        fun startunitconfigTB(builder: FlatBufferBuilder) = builder.startTable(1)
        fun addUnitconfigTRS(builder: FlatBufferBuilder, unitconfigTRS: Int) = builder.addOffset(0, unitconfigTRS, 0)
        fun createUnitconfigTRSVector(builder: FlatBufferBuilder, data: IntArray) : Int {
            builder.startVector(4, data.size, 4)
            for (i in data.size - 1 downTo 0) {
                builder.addOffset(data[i])
            }
            return builder.endVector()
        }
        fun startUnitconfigTRSVector(builder: FlatBufferBuilder, numElems: Int) = builder.startVector(4, numElems, 4)
        fun endunitconfigTB(builder: FlatBufferBuilder) : Int {
            val o = builder.endTable()
            return o
        }
        fun finishunitconfigTBBuffer(builder: FlatBufferBuilder, offset: Int) = builder.finish(offset)
        fun finishSizePrefixedunitconfigTBBuffer(builder: FlatBufferBuilder, offset: Int) = builder.finishSizePrefixed(offset)
    }
}