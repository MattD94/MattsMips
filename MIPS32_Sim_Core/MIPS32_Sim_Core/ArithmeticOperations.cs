
using System;
/// <summary>
/// All arithmetic operations are defined here in alphabetical order
/// </summary>
namespace MIPS32_Sim_Core {

    // ADD
    public class Op_add : IOperation {
        private int rs, rt, rd; // register refrences
        private UInt32 lastRd; // state of dest register at begining of operation execution

        public Op_add(int rs, int rt, int rd) {
            this.rs = rs;
            this.rt = rt;
            this.rd = rd;
        }

        /// <summary>
        /// Adds contents of rs to contents of rt and store in rd. Trap if overflow is detected.
        /// Also saves the state of rd before changing it so it may be undone.
        /// </summary>
        /// <param name="chip"></param>
        public void doOp(MIPSChip_Instance chip) {
            this.lastRd = chip.getRegister(this.rd);
            try {
                UInt32 temp = (UInt32)checked((Int32)chip.getRegister(rs) + (Int32)chip.getRegister(rt));
                chip.setRegister(rd, temp);
            } catch (OverflowException) {
                chip.interrupt();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public uint getByteCode() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reverts changes made by setting the contents of rd to the saved value.
        /// For this to work properly, doOp must have been called only once with the same chip specified here.
        /// Cannot account for any changes made to registers done by any trap handlers resulting from an overflow.
        /// </summary>
        /// <param name="chip"></param>
        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(this.rd, this.lastRd);
        }
    }

    //ADDI
    public class Op_addi : IOperation {
        private int rs, rt; // register references
        private short immediate; // signed 16bit value to add with
        private UInt32 lastRt; // state of dest register at begining of operation execution
        
        public Op_addi(int rs, int rt, short immediate) {
            this.rs = rs;
            this.rt = rt;
            this.immediate = immediate;
        }

        /// <summary>
        /// Adds contents of rs to signed 16bit value and store in rt. Trap if overflow is detected.
        /// Also saves the state of rt before changing it so it may be undone.
        /// </summary>
        /// <param name="chip"></param>
        public void doOp(MIPSChip_Instance chip) {
            lastRt = chip.getRegister(rt);
            try {
                UInt32 temp = (UInt32)checked((Int32)chip.getRegister(rs) + (Int32)immediate);
                chip.setRegister(rt, temp);
            } catch (OverflowException) {
                chip.interrupt();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public uint getByteCode() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reverts changes made by setting the contents of rt to the saved value.
        /// For this to work properly, doOp must have been called only once with the same chip specified here.
        /// Cannot account for any changes made to registers done by any trap handlers resulting from an overflow.
        /// </summary>
        /// <param name="chip"></param>
        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(this.rt, this.lastRt);
        }
    }

    //ADDIU
    public class Op_addiu : IOperation {
        private int rs, rt;
        private short immediate; // register refrences
        private UInt32 lastRt; // state of dest register at begining of operation execution

        public Op_addiu(int rs, int rt, short immediate) {
            this.rs = rs;
            this.rt = rt;
            this.immediate = immediate;
        }

        /// <summary>
        /// Adds contents of rs to 16bit value and store in rt. Does not do anything upon overflow.
        /// Also saves the state of rt before changing it so it may be undone.
        /// </summary>
        /// <param name="chip"></param>
        public void doOp(MIPSChip_Instance chip) {
            lastRt = chip.getRegister(rt);
            chip.setRegister(rt, unchecked(chip.getRegister(rs) + (UInt32)immediate));
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public uint getByteCode() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reverts changes made by setting the contents of rt to the saved value.
        /// For this to work properly, doOp must have been called only once with the same chip specified here.
        /// </summary>
        /// <param name="chip"></param>
        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(this.rt, this.lastRt);
        }
    }

    //ADDU
    public class Op_addu : IOperation {
        private int rs, rt, rd; // register refrences
        private UInt32 lastRd; // state of dest register at begining of operation execution

        public Op_addu(int rs, int rt, int rd) {
            this.rs = rs;
            this.rt = rt;
            this.rd = rd;
        }

        /// <summary>
        /// Adds contents of rs to contents of rt and store in rd.
        /// Also saves the state of rd before changing it so it may be undone.
        /// </summary>
        /// <param name="chip"></param>
        public void doOp(MIPSChip_Instance chip) {
            this.lastRd = chip.getRegister(this.rd);
            chip.setRegister(rd, unchecked(chip.getRegister(rs) + chip.getRegister(rt)));
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public uint getByteCode() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reverts changes made by setting the contents of rd to the saved value.
        /// For this to work properly, doOp must have been called only once with the same chip specified here.
        /// </summary>
        /// <param name="chip"></param>
        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(this.rd, this.lastRd);
        }
    }

    //CLO
    public class Op_clo : IOperation {
        private int rs, rd;
        private UInt32 last;

        public Op_clo(int rs, int rd) {
            this.rs = rs;
            this.rd = rd;
        }

        public void doOp(MIPSChip_Instance chip) {
            this.last = chip.getRegister(rd);
            int count = 32;
            UInt32 value = chip.getRegister(rs);
            for (int i = 31; i >= 0; i--) {
                if (value >> i != 1) {
                    count = 31 - i;
                    break;
                }
            }
            chip.setRegister(rd, (UInt32)count);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(rd, last);
        }
    }

    //CLZ
    public class Op_clz : IOperation {
        private int rs, rd;
        private UInt32 last;

        public Op_clz(int rs, int rd) {
            this.rs = rs;
            this.rd = rd;
        }

        public void doOp(MIPSChip_Instance chip) {
            this.last = chip.getRegister(rd);
            int count = 32;
            UInt32 value = chip.getRegister(rs);
            for (int i = 31; i >= 0; i--) {
                if (value >> i == 1) {
                    count = 31 - i;
                    break;
                }
            }
            chip.setRegister(rd, (UInt32)count);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(rd, last);
        }
    }

    //DIV
    public class Op_div : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_div(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            chip.HI = (UInt32)unchecked((Int32)chip.getRegister(rs) / (Int32)chip.getRegister(rt));
            chip.LO = (UInt32)unchecked((Int32)chip.getRegister(rs) % (Int32)chip.getRegister(rt));
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //DIVU
    public class Op_divu : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_divu(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            chip.HI = (UInt32)((Int32)unchecked(chip.getRegister(rs) % chip.getRegister(rt)));
            chip.LO = (UInt32)((Int32)unchecked(chip.getRegister(rs) / chip.getRegister(rt)));
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //MADD
    public class Op_madd : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_madd(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            Int64 result = (Int64)((UInt64)(lastHi << 32) | (UInt64)lastLo);
            result += unchecked((Int64)chip.getRegister(rs) * (Int64)chip.getRegister(rt));
            
            chip.HI = (UInt32)(result >> 32);
            chip.LO = (UInt32)(result & 0x0000FFFF);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //MADDU
    public class Op_maddu : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_maddu(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            UInt64 result = (UInt64)(lastHi << 32) | (UInt64)lastLo;
            result += unchecked((UInt64)chip.getRegister(rs) * (UInt64)chip.getRegister(rt));

            chip.HI = (UInt32)(result >> 32);
            chip.LO = (UInt32)(result & 0x0000FFFF);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //MSUB
    public class Op_msub : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_msub(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            Int64 result = (Int64)((UInt64)(lastHi << 32) | (UInt64)lastLo);
            result -= unchecked((Int64)chip.getRegister(rs) * (Int64)chip.getRegister(rt));

            chip.HI = (UInt32)(result >> 32);
            chip.LO = (UInt32)(result & 0x0000FFFF);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //MSUBU
    public class Op_msubu : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_msubu(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            UInt64 result = (UInt64)(lastHi << 32) | (UInt64)lastLo;
            result -= unchecked((UInt64)chip.getRegister(rs) * (UInt64)chip.getRegister(rt));

            chip.HI = (UInt32)(result >> 32);
            chip.LO = (UInt32)(result & 0x0000FFFF);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //MUL
    public class Op_mul : IOperation {
        private int rs, rt, rd;
        private UInt32 last;
        public Op_mul(int rs, int rt, int rd) {
            this.rs = rs;
            this.rt = rt;
            this.rd = rd;
        }
        public void doOp(MIPSChip_Instance chip) {
            last = chip.getRegister(rd);
            Int64 result = unchecked((Int64)chip.getRegister(rs) * (Int64)chip.getRegister(rd));
            chip.setRegister(rd, (UInt32)(result & 0x0000ffff));
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(rd, last);
        }
    }

    //MULT
    public class Op_mult : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_mult(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            Int64 result = unchecked((Int64)chip.getRegister(rs) * (Int64)chip.getRegister(rt));
            chip.HI = (UInt32)(result >> 32);
            chip.LO = (UInt32)(result & 0x0000FFFF);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //MULTU
    public class Op_multu : IOperation {
        private int rs, rt;
        private UInt32 lastHi, lastLo;
        public Op_multu(int rs, int rt) {
            this.rs = rs;
            this.rt = rt;
        }
        public void doOp(MIPSChip_Instance chip) {
            lastHi = chip.HI;
            lastLo = chip.LO;
            UInt64 result = unchecked((UInt64)chip.getRegister(rs) * (UInt64)chip.getRegister(rt));
            chip.HI = (UInt32)(result >> 32);
            chip.LO = (UInt32)(result & 0x0000FFFF);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.HI = lastHi;
            chip.LO = lastLo;
        }
    }

    //SEB
    public class Op_seb : IOperation {
        private int rt, rd;
        private UInt32 last;
        public Op_seb(int rt, int rd) {
            this.rt = rt;
            this.rd = rd;
        }
        public void doOp(MIPSChip_Instance chip) {
            last = chip.getRegister(rd);
            UInt32 temp = chip.getRegister(rt) | 0xffffff00;
            if (temp < 0xffffff80) {
                temp &= 0x000000ff;
            }
            chip.setRegister(rd, temp);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(rd, last);
        }
    }

    //SEH
    public class Op_seh : IOperation {
        private int rt, rd;
        private UInt32 last;
        public Op_seh(int rt, int rd) {
            this.rt = rt;
            this.rd = rd;
        }
        public void doOp(MIPSChip_Instance chip) {
            last = chip.getRegister(rd);
            UInt32 temp = chip.getRegister(rt) | 0xffff0000;
            if (temp < 0xffff8000) {
                temp &= 0x0000ffff;
            }

            chip.setRegister(rd, temp);
        }

        public uint getByteCode() {
            throw new NotImplementedException();
        }

        public void undoOp(MIPSChip_Instance chip) {
            chip.setRegister(rd, last);
        }
    }

    //SLT

    //SLTI
    //SLTIU
    //SLTU
    //SUB
    //SUBU
}
