<template>
  <div>
    <div v-if="!state">
      尊敬的{{quanxian_ch}},您的权限等级为{{quanxian}}
      <el-button type="info" @click="chongzhima">充值码发放</el-button>
      <el-button type="info" @click="zhuxiao" style="margin-left:100px;">注销</el-button>
      <el-tabs v-model="activeName" @tab-click="handleClick">
      <el-tab-pane label="成员管理" name="first">
        管理员
        <el-table :data="gly" style="width:100%;" stripe>
          <el-table-column prop="username" label="账号" ></el-table-column>
          <el-table-column prop="password" label="密码"></el-table-column>
          <el-table-column prop="quanxian" label="权限"></el-table-column>
          <el-table-column label="操作">
            <template slot-scope="scope">
              <el-input-number v-model="scope.row.num" :min="1" :max="100" size="mini" controls-position="right"></el-input-number>
              <el-button size="mini" type="primary" @click="handleChange(scope.row)">更改权限</el-button>
              <el-button size="mini" type="danger" @click="handleDelete(scope.row)">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <div style="margin-top:30px;"><el-button @click="handleZengjia">添加管理员</el-button></div>
        <div style="margin-top:30px;">商家</div>
        <el-table :data="sj" style="width:100%;" :row-class-name="tableRowClassName">
          <el-table-column prop="username" label="账号"></el-table-column>
          <el-table-column prop="password" label="密码"></el-table-column>
          <el-table-column prop="name" label="昵称"></el-table-column>
          <el-table-column prop="money" label="余额"></el-table-column>
          <el-table-column prop="count_pai" label="上架拍卖品次数"></el-table-column>
          <el-table-column prop="count_zhuan" label="开启专场次数"></el-table-column>
          <el-table-column prop="number" label="商户号"></el-table-column>
          <el-table-column prop="gname" label="商户名称"></el-table-column>
          <el-table-column prop="address" label="商户地址"></el-table-column>
          <el-table-column prop="phonenumber" label="电话"></el-table-column>
          <el-table-column label="操作">
          <template slot-scope="scope">
            <div v-if="scope.row.state === '3'">
              <el-button type="primary" @click="fengjin(scope.row)" size="mini">封禁</el-button>
            </div>
            <div v-if="scope.row.state === '5'">
              <el-button type="primary" @click="jiefeng(scope.row)" size="mini">解封</el-button>
            </div>
          </template>
          </el-table-column>
        </el-table>
        <div style="margin-top:30px;">用户</div>
        <el-table :data="cy" style="width:100%;" :row-class-name="tableRowClassName1">
          <el-table-column prop="username" label="账号"></el-table-column>
          <el-table-column prop="password" label="密码"></el-table-column>
          <el-table-column prop="name" label="昵称"></el-table-column>
          <el-table-column prop="money" label="余额"></el-table-column>
          <el-table-column prop="count" label="成交次数"></el-table-column>
          <el-table-column label="操作">
          <template slot-scope="scope">
            <div v-if="scope.row.state === '1'">
              <el-button type="primary" @click="fengjin(scope.row)" size="mini">封禁</el-button>
            </div>
            <div v-if="scope.row.state === '4'">
              <el-button type="primary" @click="jiefeng(scope.row)" size="mini">解封</el-button>
            </div>
          </template>
          </el-table-column>
        </el-table>
        <div style="margin-top:30px;">待审核</div>
        <el-table :data="shenhee" style="width:100%;">
          <el-table-column type="expand">
            <template slot-scope="props">
            <el-form label-position="left" inline>
              <el-form-item label="申请明细">
                <span v-for="item in props.row.source" :key="item">
                  <el-image style="width: 100px; height: 100px;" :src="item" :preview-src-list="props.row.source"></el-image>
                </span>
              </el-form-item>
            </el-form>
            </template>
          </el-table-column>
          <el-table-column prop="users" label="用户"></el-table-column>
          <el-table-column prop="gname" label="商户名"></el-table-column>
          <el-table-column prop="address" label="商户地址"></el-table-column>
          <el-table-column prop="phonenumber" label="电话号码"></el-table-column>
          <el-table-column label="操作">
          <template slot-scope="scope">
            <el-button type="primary" @click="bohui(scope.row)" size="mini">驳回</el-button>
            <el-button type="primary" @click="tongyi(scope.row)" size="mini">同意</el-button>
          </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
      <el-tab-pane label="拍卖品管理" name="second">
        <div>拍卖品</div>
        <el-table :data="pmp" style="width:100%" stripe>
          <el-table-column prop="name" label="物品名"></el-table-column>
          <el-table-column prop="type" label="物品类型"></el-table-column>
          <el-table-column prop="number" label="商家"></el-table-column>
          <el-table-column prop="startprice" label="起始价格"></el-table-column>
          <el-table-column prop="nowprice" label="当前价格"></el-table-column>
          <el-table-column prop="gapprice" label="竞价阶梯"></el-table-column>
          <el-table-column prop="cishu" label="竞价次数"></el-table-column>
          <el-table-column prop="person" label="当前出价者"></el-table-column>
          <el-table-column prop="url" label="样图">
            <template slot-scope="scope">
              <el-image :src="scope.row.url"></el-image>
            </template>
          </el-table-column>
          <el-table-column prop="state" label="状态"></el-table-column>
          <el-table-column label="操作">
          <template slot-scope="scope">
            <div v-if="scope.row.state === '拍卖中'">
              <el-button type="primary" disabled @click="paimaidelete(scope.row)" size="mini">删除</el-button>
            </div>
            <div v-else>
              <el-button type="primary" @click="paimaidelete(scope.row)" size="mini">删除</el-button>
            </div>
          </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
      <el-tab-pane label="专场管理" name="third">
        <div>专场</div>
        <el-table :data="zcp" stripe style="width: 100%;" border>
          <el-table-column prop="zname" label="专场名"></el-table-column>
          <el-table-column prop="number" label="商家"></el-table-column>
          <el-table-column prop="fzperson" label="负责人"></el-table-column>
          <el-table-column prop="name" label="物品名"></el-table-column>
          <el-table-column prop="startprice" label="起始价格"></el-table-column>
          <el-table-column prop="nowprice" label="当前价格"></el-table-column>
          <el-table-column prop="gapprice" label="竞价阶梯"></el-table-column>
          <el-table-column prop="starttime" label="开始时间"></el-table-column>
          <el-table-column prop="endtime" label="结束时间"></el-table-column>
          <el-table-column prop="person" label="出价者"></el-table-column>
          <el-table-column prop="cishu" label="出价次数"></el-table-column>
          <el-table-column label="样图">
          <template slot-scope="scope">
            <el-image :src="scope.row.url"></el-image>
          </template>
          </el-table-column>
          <el-table-column label="操作">
            <template slot-scope="scope">
              <div v-if="scope.row.state === '拍卖中'">
                <el-button type="danger" disabled @click="zcdelete(scope.row)">删除</el-button>
              </div>
              <div v-else>
                <el-button type="danger" @click="zcdelete(scope.row)">删除</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
      </el-tabs>
    </div>
    <el-dialog title="登录" :visible.sync="state">
      <el-form :model="dengluform" :rules="rules" ref="dengluform">
        <el-form-item label="账号" prop="username" style="width: 100%;margin-top:30px;">
          <el-input type="text" v-model="dengluform.username" placeholder="账号"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password" style="width: 100%;margin-top:30px;">
          <el-input type="password" v-model="dengluform.password" placeholder="密码"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button style="primary" @click="denglu()">登录</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
    <el-dialog title="添加管理员" :visible.sync="dialogVisible1">
      <el-form :model="guanli_zengjia" :rules="rules" ref="guanli_zengjia">
        <el-form-item label="账号" prop="username" style="width: 100%;margin-top:30px;">
          <el-input type="text" v-model="guanli_zengjia.username" placeholder="账号"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password" style="width: 100%;margin-top:30px;">
          <el-input type="password" v-model="guanli_zengjia.password" placeholder="密码"></el-input>
        </el-form-item>
        <el-form-item label="权限" prop="quanxian">
          <el-input-number v-model="guanli_zengjia.num" :min="quanxian + 1" :max="100"></el-input-number>
        </el-form-item>
        <el-form-item>
          <el-button style="primary" @click="tianjia()">添加</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
    <el-dialog title="充值码添加" :visible.sync="dialogVisible2">
      <el-form :model="chongzhi" :rules="rules1" ref="chongzhi">
        <el-form-item label="充值码" prop="ma" style="width:100%;margin-top:30px;">
          <el-input type="text" v-model="chongzhi.ma" placeholder="10位充值码"></el-input>
        </el-form-item>
        <el-form-item label="金额" prop="money">
          <el-input-number v-model="chongzhi.money" :min="100" :step="100" :max="1000"></el-input-number>
        </el-form-item>
        <el-button type="info" @click="chongzhitijiao">添加</el-button>
      </el-form>
    </el-dialog>
  </div>
</template>
<script>
import qs from 'qs'
export default {
  name: 'Houtai',
  data () {
    var validateUser = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('账号不能为空'))
      } else if (!this.pattern1.test(value)) {
        callback(new Error('账号格式不正确'))
      } else {
        callback()
      }
    }
    var validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('密码不能为空'))
      } else if (!this.pattern1.test(value)) {
        callback(new Error('密码格式不正确'))
      } else {
        callback()
      }
    }
    var validateChong = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('充值码不能为空'))
      } else if (!this.pattern2.test(value)) {
        callback(new Error('充值码格式不正确'))
      } else {
        callback()
      }
    }
    return {
      chongzhi: {ma: '', money: 100},
      guanli_zengjia: {username: '', password: '', num: 1},
      dialogVisible1: false,
      dialogVisible2: false,
      state: false,
      quanxian: 100,
      quanxian_ch: '',
      gly: [],
      cy: [],
      sj: [],
      shenhee: [],
      pmp: [],
      zcp: [],
      dengluform: {username: '', password: ''},
      pattern1: /^[a-zA-Z0-9]{4,18}$/,
      pattern2: /^[0-9a-zA-Z]{10}$/,
      activeName: 'first',
      rules: {
        username: [
          { validator: validateUser, trigger: 'blur' }
        ],
        password: [
          { validator: validatePass, trigger: 'blur' }
        ]
      },
      rules1: {
        ma: [
          { validator: validateChong, trigger: 'blur' }
        ]
      },
      chengyuan: []
    }
  },
  methods: {
    zhuxiao () {
      this.$axios({method: 'post', url: 'product/guanli/zhuxiao'})
        .then(response => {
          if (response.status === 200) {
            this.$router.go(0)
          }
        })
    },
    // arraySpanMethod ({row, column, rowIndex, columnIndex}) {
    //   var args = ''
    //   var jishu = 1
    //   if (columnIndex === 0 || columnIndex === 1 || columnIndex === 2) {
    //     if (row.zname !== args) {
    //       args = row.zname
    //       jishu = 1
    //       return {
    //         rowspan: jishu,
    //         colspan: 1
    //       }
    //     } else {
    //       args = row.zname
    //       jishu++
    //       return {
    //         rowspan: 0,
    //         colspan: 0
    //       }
    //     }
    //   }
    // },
    zcdelete (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/zcdelete', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else {
            this.zhuanchangpin()
          }
        })
    },
    chongzhitijiao () {
      this.$refs['chongzhi'].validate((valid) => {
        if (valid) {
          var data = qs.stringify(this.chongzhi)
          this.$axios({method: 'post', url: 'product/guanli/chongzhi', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
            .then(response => {
              if (response.data.code === '300') {
                this.$message.error(response.data.message)
              } else {
                this.$message.success('操作成功')
                this.chongzhi = {ma: '', money: 100}
              }
            })
        } else {
          return false
        }
      })
    },
    chongzhima () {
      this.chongzhi = {ma: '', money: 100}
      this.dialogVisible2 = true
    },
    shenheehuoqu () {
      this.$axios({method: 'post', url: 'product/guanli/cwsjhuoqu'})
        .then(response => {
          this.shenhee = []
          for (var i = 0; i < response.data.length; i++) {
            var source = response.data[i].source
            var ss = source.split(',')
            response.data[i].source = []
            for (var j = 0; j < ss.length; j++) {
              response.data[i].source[j] = ss[j]
            }
            this.shenhee.push(response.data[i])
            console.log(this.shenhee)
          }
        })
    },
    paimaidelete (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/paimaidelete', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else if (response.data.code === '200') {
            this.$message.success('删除成功')
            this.paimaipin()
          }
        })
    },
    bohui (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/bohui', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else if (response.data.code === '200') {
            this.$message.success('操作成功')
            this.shenheehuoqu()
            this.chengyuanhuoqu()
          }
        })
    },
    tongyi (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/tongyi', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else if (response.data.code === '200') {
            this.$message.success('操作成功')
            this.shenheehuoqu()
            this.chengyuanhuoqu()
          }
        })
    },
    jiefeng (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/jiefeng', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else if (response.data.code === '200') {
            this.$message.success('解封成功')
            this.chengyuanhuoqu()
          }
        })
    },
    fengjin (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/fengjin', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else if (response.data.code === '200') {
            this.$message.success('封禁成功')
            this.chengyuanhuoqu()
          }
        })
    },
    tableRowClassName1 (row, rowIndex) {
      if (row.row.state === '4') {
        return 'warning-row'
      }
      return ''
    },
    tableRowClassName (row, rowIndex) {
      if (row.row.state === '5') {
        return 'warning-row'
      }
      return ''
    },
    tianjia () {
      this.$refs['guanli_zengjia'].validate((valid) => {
        if (valid) {
          var data = qs.stringify(this.guanli_zengjia)
          this.$axios({method: 'post', url: 'product/guanli/tianjia', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
            .then(response => {
              if (response.data.code === '200') {
                this.guanli_zengjia = {}
                this.huoqu()
                this.dialogVisible1 = false
                this.$message.success('添加成功')
              } else {
                this.$message.error(response.data.message)
              }
            })
        }
      })
    },
    handleZengjia () {
      this.guanli_zengjia = {username: '', password: '', num: 1}
      this.dialogVisible1 = true
    },
    handleChange (row) {
      if (row.num === row.quanxian) {
        this.$message.warning('权限没有变化')
      } else if (row.num === undefined) {
        this.$message.warning('请输入要改变的权限值')
      } else {
        var data = qs.stringify(row)
        this.$axios({method: 'post', url: 'product/guanli/quanxianchange', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
          .then(response => {
            if (response.data.code === '300') {
              this.$message.error(response.data.message)
            } else {
              this.$message.success('更改权限成功')
              this.chengyuanhuoqu()
            }
          })
      }
    },
    handleDelete (row) {
      var data = qs.stringify(row)
      this.$axios({method: 'post', url: 'product/guanli/guanlidelete', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '300') {
            this.$message.error(response.data.message)
          } else {
            this.$message.success('删除成功')
            this.chengyuanhuoqu()
          }
        })
    },
    denglu () {
      this.$refs['dengluform'].validate((valid) => {
        if (valid) {
          var data = qs.stringify(this.dengluform)
          this.$axios({method: 'post', url: 'product/guanli/denglu', data: data})
            .then(response => {
              if (response.data.Code === 200) {
                this.$message.success(response.data.Mess)
                this.state = false
                this.panduan()
              } else if (response.data.Code === 300) {
                this.$message.error(response.data.Mess)
              }
            })
        } else {
          return false
        }
      })
    },
    panduan () {
      this.$axios({method: 'post', url: 'product/guanli/panduan'})
        .then(response => {
          if (response.data === 'false') {
            this.state = true
          } else {
            this.state = false
            this.quanxian = response.data.Table[0].quanxian
            if (this.quanxian === 0) {
              this.quanxian_ch = '超级管理员'
            } else if (this.quanxian <= 10) {
              this.quanxian_ch = '高级管理员'
            } else {
              this.quanxian_ch = '低级管理员'
            }
            this.huoqu()
          }
        })
    },
    huoqu () {
      this.chengyuanhuoqu()
      this.shenheehuoqu()
      this.paimaipin()
      this.zhuanchangpin()
    },
    chengyuanhuoqu () {
      this.$axios({method: 'post', url: 'product/guanli/chengyuanhuoqu'})
        .then(response => {
          this.gly = []
          this.cy = []
          this.sj = []
          var gly = response.data.gly.Table
          for (var i = 0; i < gly.length; i++) {
            this.gly.push(gly[i])
          }
          var cy = response.data.cy
          for (var j = 0; j < cy.length; j++) {
            this.cy.push(cy[j])
          }
          var sj = response.data.sj
          for (var k = 0; k < sj.length; k++) {
            this.sj.push(sj[k])
          }
        })
    },
    handleClick (tab) {
      this.activeName = tab.name
    },
    paimaipin () {
      this.$axios({method: 'post', url: 'product/guanli/paimaipin'})
        .then(response => {
          this.pmp = []
          var data = response.data.Table
          for (var i = 0; i < data.length; i++) {
            this.pmp.push(data[i])
          }
          for (var j = 0; j < this.pmp.length; j++) {
            var starttime = this.pmp[j].starttime
            var endtime = this.pmp[j].endtime

            // 开始时间 date2
            var time1 = starttime.split('T')
            var time2 = time1[0].split('-')
            var time3 = time1[1].split(':')
            var date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            var date2 = date1.getTime()

            // 結束时间 date3
            time1 = endtime.split('T')
            time2 = time1[0].split('-')
            time3 = time1[1].split(':')
            date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            var date3 = date1.getTime()

            // 现在时间 date4
            var date4 = new Date().getTime()

            if (date4 < date2) {
              this.pmp[j].state = '未开始'
            } else if (date4 > date3) {
              this.pmp[j].state = '已结束'
            } else {
              this.pmp[j].state = '拍卖中'
            }
          }
        })
    },
    zhuanchangpin () {
      this.$axios({method: 'post', url: 'product/guanli/zhuanchangpin'})
        .then(response => {
          this.zcp = []
          var data = response.data
          for (var i = 0; i < data.length; i++) {
            this.zcp.push(data[i])
          }
          for (var j = 0; j < this.zcp.length; j++) {
            var starttime = this.zcp[j].starttime
            var endtime = this.zcp[j].endtime

            var time1 = endtime.split(' ')
            var time2 = time1[0].split('/')
            var time3 = time1[1].split(':')
            var date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            // endtime
            var date2 = date1.getTime()

            // 现在的时间
            var date = new Date().getTime()

            time1 = starttime.split(' ')
            time2 = time1[0].split('/')
            time3 = time1[1].split(':')
            date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            // starttime
            var date3 = date1.getTime()
            if (date < date3) {
              this.zcp[j].state = '未开始'
            } else if (date > date2) {
              this.zcp[j].state = '已结束'
            } else {
              this.zcp[j].state = '拍卖中'
            }
          }
          // var z1 = response.data.z1.Table
          // var z2 = response.data.z2
          // for (var i = 0; i < z1.length; i++) {
          //   this.zcp.push(z1[i])
          // }
          // for (var j = 0; j < this.zcp.length; j++) {
          //   this.zcp[j].childern = []
          //   var biaoji = false
          //   for (var k = 0; k < z2.length; k++) {
          //     for (var h in z2[k]) {
          //       if (h === this.zcp[j].zname) {
          //         biaoji = true
          //         for (var l = 0; l < z2[k][h].Table.length; l++) {
          //           this.zcp[j].childern.push(z2[k][h].Table[l])
          //         }
          //       }
          //     }
          //     if (biaoji === true) {
          //       break
          //     } else {
          //       continue
          //     }
          //   }
          // }
        })
    }
  },
  mounted () {
    this.panduan()
  }
}
</script>
<style scoped>
  .el-table >>> .warning-row {
    background:LightGrey;
  }
</style>
