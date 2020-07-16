<template>
  <div v-if="start" style="margin-top:30px">
    <div><hr style="height:3px;border:none;border-top:3px solid #FF9797 ;"/></div>
    <div style="margin-top:30px">
      <el-col :span="8">
        <div style="height:406px;width:402px;border:1px solid #dddddd;float:left;padding:10px">
        <img :src="table.url" style="width:100%;heigth:100%;"/>
        </div>
      </el-col>
      <el-col :span="16">
        <el-card>
          <div slot="header" style="magin-left:20px;text-align:left;font-size:16px">{{table.name}}</div>
          <div>
            <el-row>
              <el-col :span="2">
                <div style="background-color:#d2375f;color:#FFF;height:40px;width:34px;font-size:14px;text-align:center;padding-top:8px;padding-right:7px;padding-bottom:10px;padding-left:8px;vertical-align:middle">{{state}}</div>
              </el-col>
              <el-col :span="22">
                <div>
                  <div style="margin-left:20px;text-align:left">
                    {{jtime}}&nbsp;
                    <span style="color:red">{{day}}&nbsp;</span>天&nbsp;
                    <span style="color:red">{{hour}}&nbsp;</span>时&nbsp;
                    <span style="color:red">{{minute}}&nbsp;</span>分&nbsp;
                    <span style="color:red">{{mill}}&nbsp;</span>秒&nbsp;
                  </div>
                  <div style="margin-left:20px;text-align:left;margin-top:15px">
                    开始时间: {{table.starttime === null ? null :table.starttime.replace("T", " ")}}
                    <span style="margin-left:100px">结束时间: {{table.endtime === null ? null : table.endtime.replace("T", " ")}}</span>
                  </div>
                </div>
              </el-col>
            </el-row>
          </div>
        </el-card>
        <span style="float:left;margin-left:20px;margin-top:20px;">
          出价次数: <span style="color:red">{{table.cishu}}</span>次
        </span>
        <span style="float:right;margin-right:30%;margin-top:20px">
          领先者: <span style="color:red">{{table.person}}</span>
        </span>
        <div style="margin-top:30px;margin-left:5%;">
          <el-table style="width:100%;margin-top:70px;" :data="tableData">
            <el-table-column prop="nowprice" width="180" label="当前价">
            </el-table-column>
            <el-table-column prop="startprice" width="180" label="起拍价">
            </el-table-column>
            <el-table-column prop="gapprice" width="180" label="竞价阶梯">
            </el-table-column>
          </el-table>
          <el-input-number style="float:left;margin-top:20px;" v-model="money" @change="handleChange" @blur="handleBlur" :step="this.table.gapprice" :min="this.table.nowprice + this.table.gapprice" :max="this.table.nowprice * 100" label="付钱">
          </el-input-number>
          <div style="float:left;margin-top:20px; margin-left:30px;">
          <el-button style="background:#d2375f;color:#fff" @click="fuqian">
            确认出价
          </el-button>
          </div>
        </div>
      </el-col>
    </div>
  </div>
</template>
<script>
export default {
  name: 'Buy',
  data () {
    return {
      start: true,
      table: {},
      state: '',
      state1: '',
      jtime: '',
      day: '',
      hour: '',
      minute: '',
      mill: '',
      date2: 0,
      date3: 0,
      timer: '',
      tableData: [{
        nowprice: 0,
        startprice: 0,
        gapprice: 0
      }],
      money: 0,
      name: '',
      zname: '',
      fenge: ''
    }
  },
  methods: {
    zhuanchaxun () {
    },
    chaxun (baseurl) {
      var url = ''
      if (baseurl === 'putong') {
        url = 'product/putong/' + encodeURIComponent(this.name)
      } else if (baseurl === 'zhuanchang') {
        url = 'product/zhuanchang/' + encodeURIComponent(this.name) + '/' + encodeURIComponent(this.zname)
      }
      this.$axios({method: 'post', url: url})
        .then(response => {
          this.table = response.data.Table[0]
          this.tableData[0].nowprice = this.table.nowprice
          this.tableData[0].startprice = this.table.startprice
          this.tableData[0].gapprice = this.table.gapprice
          this.money = this.table.nowprice
          var time1 = this.table.endtime.split('T')
          var time2 = time1[0].split('-')
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
          time1 = this.table.starttime.split('T')
          time2 = time1[0].split('-')
          time3 = time1[1].split(':')
          date1 = new Date()
          date1.setFullYear(time2[0], time2[1] - 1, time2[2])
          date1.setHours(time3[0])
          date1.setMinutes(time3[1])
          date1.setSeconds(time3[2])
          // starttime
          var date3 = date1.getTime()
          if (date3 > date) {
            this.state = '预展中'
            this.jtime = '距开拍:'
            var dd = date3 - date
            // 计算出相差天书
            this.day = Math.floor(dd / (24 * 3600 * 1000))
            // 计算出小时数
            var level1 = dd % (24 * 3600 * 1000)
            this.hour = Math.floor(level1 / (3600 * 1000))
            // 计算出相差分钟数
            var level2 = level1 % (3600 * 1000)
            this.minute = Math.floor(level2 / (60 * 1000))
            // 计算相差秒数
            var level3 = level2 % (60 * 1000)
            this.mill = Math.round(level3 / 1000)
            this.date3 = date3
            this.timer = setInterval(this.time1, 1000)
          } else if (date2 < date) {
            this.state = '已结束'
            this.jtime = '距结束:'
            this.day = 0
            this.hour = 0
            this.minute = 0
            this.mill = 0
          } else {
            this.state = '拍卖中'
            this.jtime = '距结束:'
            var ddd = date2 - date
            // 计算出相差天书
            this.day = Math.floor(ddd / (24 * 3600 * 1000))
            // 计算出小时数
            var level4 = ddd % (24 * 3600 * 1000)
            this.hour = Math.floor(level4 / (3600 * 1000))
            // 计算出相差分钟数
            var level5 = level4 % (3600 * 1000)
            this.minute = Math.floor(level5 / (60 * 1000))
            // 计算相差秒数
            var level6 = level5 % (60 * 1000)
            this.mill = Math.round(level6 / 1000)
            this.date2 = date2
            this.timer = setInterval(this.time2, 1000)
          }
          // if (baseurl === 'zhuanchang') {
          //   var uu = this.table.url
          //   this.table.url = this.table.z_url
          //   this.table.z_url = uu
          // }
        })
    },
    handleChange (currentValue, oldValue) {
    },
    handleBlur () {
      var cha = this.money - this.table.nowprice
      if (cha < 0 || cha % this.table.gapprice !== 0) {
        this.$message.error('数据不正确,请重新输入')
        this.money = this.table.nowprice + this.table.gapprice
      }
    },
    fuqian () {
      var cha = this.money - this.table.nowprice
      if (cha < 0 || cha % this.table.gapprice !== 0) {
        this.$message.error('数据不正确,请重新输入')
        this.money = this.table.nowprice + this.table.gapprice
      } else {
        if (this.fenge === 'putong') {
          var url = 'product/fuqian/' + this.table.id + '/' + this.money
          this.$axios({method: 'post', url: url})
            .then(response => {
              var code = response.data.Code
              if (code === 100) {
                this.$message.error(response.data.Mess)
                setTimeout(this.$router.push({name: 'Login'}), 2000)
              }
              if (code === 200) {
                this.$message.error(response.data.Mess)
              }
              if (code === 300 || code === 400) {
                this.$message.error(response.data.Mess)
              }
              if (code === 500) {
                this.$message({message: response.data.Mess, type: 'success'})
              }
            })
        } else if (this.fenge === 'zhuanchang') {
          var zname = encodeURIComponent(this.table.zname)
          var name = encodeURIComponent(this.table.name)
          var uurl = 'product/fuqian1/' + zname + '/' + name + '/' + this.money
          this.$axios({method: 'post', url: uurl})
            .then(response => {
              var code = response.data.Code
              if (code === 100) {
                this.$message.error(response.data.Mess)
                setTimeout(this.$router.push({name: 'Login'}), 2000)
              }
              if (code === 200) {
                this.$message.error(response.data.Mess)
              }
              if (code === 300 || code === 400) {
                this.$message.error(response.data.Mess)
              }
              if (code === 500) {
                this.$message({message: response.data.Mess, type: 'success'})
              }
            })
        }
      }
    },
    time1 () {
      var date = new Date().getTime()
      var dd = this.date3 - date
      // 计算出相差天书
      this.day = Math.floor(dd / (24 * 3600 * 1000))
      // 计算出小时数
      var level1 = dd % (24 * 3600 * 1000)
      this.hour = Math.floor(level1 / (3600 * 1000))
      // 计算出相差分钟数
      var level2 = level1 % (3600 * 1000)
      this.minute = Math.floor(level2 / (60 * 1000))
      // 计算相差秒数
      var level3 = level2 % (60 * 1000)
      this.mill = Math.round(level3 / 1000)
    },
    time2 () {
      var date = new Date().getTime()
      var dd = this.date2 - date
      // 计算出相差天书
      this.day = Math.floor(dd / (24 * 3600 * 1000))
      // 计算出小时数
      var level1 = dd % (24 * 3600 * 1000)
      this.hour = Math.floor(level1 / (3600 * 1000))
      // 计算出相差分钟数
      var level2 = level1 % (3600 * 1000)
      this.minute = Math.floor(level2 / (60 * 1000))
      // 计算相差秒数
      var level3 = level2 % (60 * 1000)
      this.mill = Math.round(level3 / 1000)
    }
  },
  mounted: function () {
    if (this.$route.query.method === 'putong') {
      this.name = this.$route.query.name
      this.fenge = 'putong'
      this.chaxun('putong')
    }
    if (this.$route.query.method === 'zhuanchang') {
      this.name = this.$route.query.name
      this.zname = this.$route.query.zname
      this.fenge = 'zhuanchang'
      this.chaxun('zhuanchang')
    }
  },
  beforeDestroy () {
    clearInterval(this.timer)
  }
}
</script>
<style scoped>

</style>
