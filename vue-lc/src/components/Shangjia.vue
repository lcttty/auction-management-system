<template>
  <div>
    <el-tabs v-model="activeName" @tab-click="handleClick">
      <el-tab-pane label="我的信息" name="first">
        <div v-if="isfirst">
          <el-row>
            <el-col :span="4">
              <el-menu
                :default-active="activeIndex"
                class="categories"
                @select="handleSelect"
                active-text-color="red"
                >
                <el-menu-item index="1">
                  <i class="el-icon-menu"></i>
                  <span slot="title">信息管理</span>
                </el-menu-item>
                <el-menu-item index="2">
                  <i class="el-icon-menu"></i>
                  <span slot="title">提现服务</span>
                </el-menu-item>
              </el-menu>
            </el-col>
            <el-col :span="20" style="margin-left:16.7%;margin-top:40px;">
              <div v-if="activeIndex === '1'" style="border:1px solid #e3e3e3;border-radius:0px;height:250px;">
                <div v-if="changeid">
                    <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">公司名称:<span style="margin-left:30px;color:red">{{ xinxi.gname }}</span></div>
                    <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">公司地址:<span style="margin-left:30px;color:red">{{ xinxi.address }}</span></div>
                    <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">公司电话:<span style="margin-left:30px;color:red">{{ xinxi.phonenumber }}</span></div>
                    <el-button style="margin-top: 43px;" @click="genggai">信息更改</el-button>
                </div>
                <div v-else style="border:1px solid #e3e3e3;border-radius:0px;height:250px;">
                  <el-form label-width="80px" :model="formx" :rules="rule1" ref="formx">
                    <el-form-item label="公司名称" prop="gname" style="width: 40%;margin-top:30px;">
                      <el-input type="text" v-model="formx.gname"></el-input>
                    </el-form-item>
                    <el-form-item label="公司地址" prop="address" style="width: 40%;">
                      <el-input type="text" v-model="formx.address"></el-input>
                    </el-form-item>
                    <el-form-item label="公司电话" prop="phonenumber" style="width: 40%;">
                      <el-input type="text" v-model="formx.phonenumber"></el-input>
                    </el-form-item>
                    <el-button @click="changexinxi">提交</el-button>
                    <el-button @click="changeid = true">取消</el-button>
                  </el-form>
                </div>
              </div>
              <div v-if="activeIndex === '2'" style="border:1px solid #e3e3e3;border-radius:0px;height:250px;">
                <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">您的余额:<span style="margin-left:30px;color:red">{{ xinxi.money }}</span></div>
                <el-form label-width="80px" :model="formt" :rules="rule2" ref="formt" style="text-align:left;margin-left:30px;font-size:20px;margin-top:30px;">
                  <el-form-item label="银行卡号" prop="cardid" style="width:40%;">
                    <el-input type="text" v-model="formt.cardid"></el-input>
                  </el-form-item>
                  <el-form-item label="提现金额" prop="money" style="width:40%;">
                    <el-input type="text" v-model="formt.money"></el-input>
                  </el-form-item>
                  <el-button @click="tixian" style="margin-left:45%;">提现</el-button>
                </el-form>
              </div>
            </el-col>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane label="拍卖品管理" name="second">
        <div v-if="issecond">
          <el-row>
            <el-col :span="4">
              <el-menu
                :default-active="activeIndex1"
                class="categories"
                @select="handleSelect1"
                active-text-color="red"
                >
                <el-menu-item index="1">
                  <i class="el-icon-menu"></i>
                  <span slot="title">拍卖品上架</span>
                </el-menu-item>
                <el-menu-item index="2">
                  <i class="el-icon-menu"></i>
                  <span slot="title">竞拍信息</span>
                </el-menu-item>
              </el-menu>
            </el-col>
            <el-col :span="20" style="margin-left:16.7%;margin-top:40px;">
              <div v-if="activeIndex1 === '1'">
                <el-row>
                  <el-col :span="8" v-for="item in paimai_card.slice((currentPage - 1) * 6, currentPage * 6)" :key="item.name">
                    <el-card :body-style="{ padding: '0px' }" shadow="never">
                      <img :src="item.url" class="image">
                      <div style="padding: 14px;">
                        <span>{{ item.name.length > 20 ? item.name.slice(0, 20) + "..." : item.name }}</span>
                        <div>{{ item.state }}</div>
                        <div class="bottom clearfix">
                          <time class="time"> {{ item.starttime }} - {{ item.endtime }}</time>
                          <el-button type="text" class="button" @click="link(item.name)">查看详细</el-button>
                        </div>
                        <div v-if="item.state === '拍卖中'">
                        <el-button type="danger" @click="del(item.name, item.number, item.url)" disabled>删除</el-button>
                        </div>
                        <div v-else>
                        <el-button type="danger" @click="del(item.name, item.number, item.url)">删除</el-button>
                        </div>
                      </div>
                    </el-card>
                  </el-col>
                </el-row>
                <el-pagination layout="prev, pager, next" :total="cc" @current-change="current_change">
                </el-pagination>
                <el-button @click="paimai_shangjia">上架拍卖品</el-button>
              </div>
              <div v-if="activeIndex1 === '2'">
                <el-table :data="jingpaicard" style="width: 100%">
                  <el-table-column prop="name" label="物品名" width="180">
                  </el-table-column>
                  <el-table-column prop="price" label="成交价格" width="180">
                  </el-table-column>
                  <el-table-column prop="time" label="时间" width="180">
                  </el-table-column>
                  <el-table-column prop="mbstate" label="成交状态" width="180">
                  </el-table-column>
                </el-table>
              </div>
            </el-col>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane label="专场管理" name="third">
        <div v-if="isthird">
          <el-row>
            <el-col :span="6" v-for="item in zhuanchang_card.slice((currentPage_zc - 1) * 12, currentPage_zc * 12)" :key="item.zname">
              <el-card :body-style="{ padding: '0px' }" shadow="never">
                      <img :src="item.url" class="image">
                      <div style="padding: 14px;">
                        <span>{{ item.zname.length > 20 ? item.zname.slice(0, 20) + "..." : item.zname }}</span>
                        <div>{{ item.state }}</div>
                        <div class="bottom clearfix">
                          <time class="time"> {{ item.starttime }} - {{ item.endtime }}</time>
                          <div><el-button type="text" @click="zhuanchang_paimai_kaiqi(item.zname)" class="button">添加拍卖品</el-button></div>
                        </div>
                        <div>
                          <div v-if="item.state === '专场进行中'">
                            <el-button type="danger" disabled @click="zc_delete(item.zname, item.url)">删除</el-button>
                          </div>
                          <div v-else>
                            <el-button type="danger" @click="zc_delete(item.zname, item.url)">删除</el-button>
                          </div>
                        </div>
                      </div>
              </el-card>
            </el-col>
          </el-row>
          <div v-if="zhuanchang_card_length !== 0">
            <el-pagination layout="prev, pager, next" :total="cc1" @current-change="current_change_zc">
            </el-pagination>
          </div>
          <el-button @click="zhuanchangbutton">添加专场</el-button>
        </div>
      </el-tab-pane>
    </el-tabs>
    <el-dialog title="专场拍卖品" :visible.sync="dialogFormVisible2">
      <el-row>
        <el-col :span="8" v-for="item in zcpm.slice((currentPage_zhuanchang - 1) * 6, currentPage_zhuanchang * 6)" :key="item.name">
          <el-card :body-style="{ padding: '0px' }" shadow="never">
            <img :src="item.url" class="image">
              <div style="padding: 14px;">
                <span>{{ item.name.length > 20 ? item.name.slice(0, 20) + "..." : item.name }}</span>
                <div class="bottom clearfix">
                <time class="time"> {{ item.starttime }} - {{ item.endtime }}</time>
                <el-button type="text" class="button" @click="linkz(item.name, zhuanchang_zname)">查看详细</el-button>
                <div v-if="item.state === '拍卖中'">
                  <el-button type="danger" disabled @click="zhuanchang_delete(item.zname, item.number, item.url, item.name)">删除</el-button>
                </div>
                <div v-else>
                  <el-button type="danger" @click="zhuanchang_delete(item.zname, item.number, item.url, item.name)">删除</el-button>
                </div>
                </div>
              </div>
          </el-card>
        </el-col>
      </el-row>
      <el-pagination layout="prev, pager, next" :total="cc2" @current-change="current_change_zhuanchang">
      </el-pagination>
      <el-button @click="paimai_zc_shangjia">上架专场拍卖品</el-button>
    </el-dialog>
    <el-dialog title="添加专场拍卖品" :visible.sync="dialogFormVisible3">
      <el-form :model="zhuanchang_form" ref="zhuanchang_form">
        <el-form-item label="拍卖品名" placeholder="请输入拍卖品名" prop="name">
          <el-input type="text" v-model="zhuanchang_form.name"></el-input>
        </el-form-item>
        <el-form-item label="起拍价格" prop="startprice">
          <el-input-number :min="500" :max="100000" :step="100" v-model="zhuanchang_form.startprice" label="竞拍起始价格"></el-input-number>
        </el-form-item>
        <el-form-item label="竞价阶梯" prop="gapprice">
            <el-input-number :min="100" :max="10000" :step="100" v-model="zhuanchang_form.gapprice" label="竞拍价格阶梯"></el-input-number>
        </el-form-item>
        <el-form-item label="开始时间" prop="time">
          <el-date-picker
            v-model="zhuanchang_form.time"
            type="datetimerange"
            range-separator="至"
            value-format="yyyy-MM-dd HH:mm:ss"
            start-placeholder="开始日期"
            end-placeholder="结束日期">
          </el-date-picker>
        </el-form-item>
      </el-form>
      <el-upload
        class="avatar-uploader"
        :with-credentials="true"
        action="http://localhost:55587/api/product/cwsj-upload"
        :http-request="upload_zhuanchang"
        :on-remove="onRemove_zhuanchang"
        :file-list="fileList_zhuanchang"
        :before-upload="beforeAvatarUpload_zhuanchang"
        :on-exceed="exceedUpload_zhuanchang"
        :limit="1"
        >
        <el-button size="small" type="primary">点击上传</el-button>
      </el-upload>
      <div slot="footer" class="dialog-footer">
        <el-button @click="shangjiaquxiao_zhuanchang">取消</el-button>
        <el-button type="primary" @click="shangjia_zhuanchang">确定</el-button>
      </div>
    </el-dialog>
    <el-dialog title="添加专场" :visible.sync="dialogFormVisible1">
      <el-form :model="zhuanchang" ref="zhuanchang">
        <el-form-item label="专场名" placeholder="请输入专场名" prop="zname">
          <el-input type="text" v-model="zhuanchang.zname"></el-input>
        </el-form-item>
        <el-form-item label="专场负责人" placeholder="请输入专场负责人" prop="fzperson">
          <el-input type="text" v-model="zhuanchang.fzperson"></el-input>
        </el-form-item>
        <el-form-item label="专场时间" prop="time">
          <el-date-picker
            v-model="zhuanchang.time"
            type="datetimerange"
            range-separator="至"
            value-format="yyyy-MM-dd HH:mm:ss"
            start-placeholder="开始日期"
            end-placeholder="结束日期">
          </el-date-picker>
        </el-form-item>
        <el-upload
        class="avatar-uploader"
        :with-credentials="true"
        action="http://localhost:55587/api/product/shangjia/zc-upload"
        :http-request="upload_zc"
        :on-remove="onRemove_zc"
        :file-list="fileList_zc"
        :before-upload="beforeAvatarUpload_zc"
        :on-exceed="exceedUpload_zc"
        :limit="1"
        >
        <el-button size="small" type="primary">点击上传</el-button>
      </el-upload>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="zhuanchangquxiao">取消</el-button>
        <el-button type="primary" @click="zhuanchangsubmit">确定</el-button>
      </div>
    </el-dialog>
    <el-dialog title="上架拍卖品" :visible.sync="dialogFormVisible">
      <el-form :model="pai_mai" ref="pai_mai">
        <el-form-item label="物品名称" prop="name">
          <el-input type="text" v-model="pai_mai.name"></el-input>
        </el-form-item>
        <el-form-item label="物品类型" prop="type">
          <el-select v-model="pai_mai.type" placeholder="请选择">
            <el-option
              v-for="item in options_type"
              :key="item.value"
              :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="起拍价格" prop="startprice">
          <el-input-number :min="500" :max="100000" :step="100" v-model="pai_mai.startprice" label="竞拍起始价格"></el-input-number>
        </el-form-item>
        <el-form-item label="竞价阶梯" prop="gapprice">
            <el-input-number :min="100" :max="10000" :step="100" v-model="pai_mai.gapprice" label="竞拍价格阶梯"></el-input-number>
        </el-form-item>
        <el-form-item label="开始时间" prop="time">
          <el-date-picker
            v-model="pai_mai.time"
            type="datetimerange"
            range-separator="至"
            value-format="yyyy-MM-dd HH:mm:ss"
            start-placeholder="开始日期"
            end-placeholder="结束日期">
          </el-date-picker>
        </el-form-item>
      </el-form>
      <el-upload
        class="avatar-uploader"
        :with-credentials="true"
        action="http://localhost:55587/api/product/cwsj-upload"
        :http-request="upload"
        :on-remove="onRemove"
        :file-list="fileList"
        :before-upload="beforeAvatarUpload"
        :on-exceed="exceedUpload"
        :limit="1"
        >
        <el-button size="small" type="primary">点击上传</el-button>
      </el-upload>
      <div slot="footer" class="dialog-footer">
        <el-button @click="shangjiaquxiao">取消</el-button>
        <el-button type="primary" @click="shangjia_paimai">确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import qs from 'qs'
export default {
  name: 'Shangjia',
  data () {
    var validategname = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入公司名称'))
      } else if (!this.pattern_wenzi.test(value)) {
        callback(new Error('名称格式不正确'))
      } else {
        callback()
      }
    }
    var validateaddress = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入公司地址'))
      } else if (!this.pattern_wenzi.test(value)) {
        callback(new Error('地址格式不正确'))
      } else {
        callback()
      }
    }
    var validatephonenumber = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入公司电话'))
      } else if (!this.pattern_phonenumber.test(value)) {
        callback(new Error('电话格式不正确'))
      } else {
        callback()
      }
    }
    var validatecard = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入银行卡号'))
      } else if (!this.pattern_cardid.test(value)) {
        callback(new Error('银行卡号格式不正确'))
      } else {
        callback()
      }
    }
    var validatemoney = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入提现的金额'))
      } else if (!this.pattern_money.test(value)) {
        callback(new Error('金额格式不正确'))
      } else {
        callback()
      }
    }
    return {
      zhuanchang_zname: '',
      zhuanchang_form: {},
      zhuanchang: {},
      zhuanchang_card: [],
      zhuanchang_card_length: 0,
      options_type: [{value: '中国绘画'}, {value: '古瓷杂项'}, {value: '酒品'}, {value: '西画雕塑'}, {value: '当代工艺'}, {value: '书法篆刻'}],
      state: 0,
      activeName: 'first',
      isfirst: true,
      issecond: false,
      isthird: false,
      activeIndex: '1',
      activeIndex1: '1',
      xinxi: {gname: '', address: '', phonenumber: '', money: 0},
      changeid: true,
      formx: {gname: '', address: '', phonenumber: ''},
      formt: {cardid: '', money: ''},
      jingpaicard: [],
      pai_mai: {name: '', type: '', startprice: 0, gapprice: 0, time: []},
      paimai_card: [],
      paimai_card_length: 0,
      cc: 0,
      cc1: 0,
      cc2: 0,
      currentPage: 1,
      currentPage_zc: 1,
      currentPage_zhuanchang: 1,
      zcpm: [],
      zcpm_length: 0,
      rule1: {
        gname: [
          { validator: validategname, trigger: 'blur' }
        ],
        address: [
          { validator: validateaddress, trigger: 'blur' }
        ],
        phonenumber: [
          { validator: validatephonenumber, trigger: 'blur' }
        ]
      },
      rule2: {
        cardid: [
          { validator: validatecard, trigger: 'blur' }
        ],
        money: [
          { validator: validatemoney, trigger: 'blur' }
        ]
      },
      fileList: [],
      fileList_zc: [],
      fileList_zhuanchang: [],
      dialogFormVisible: false,
      dialogFormVisible1: false,
      dialogFormVisible2: false,
      dialogFormVisible3: false,
      pattern_phonenumber: /^[0-9]{11}$/,
      pattern_wenzi: /^.{0,30}$/,
      pattern_money: /^[1-9][0-9]{0,9}$/,
      pattern_cardid: /^[0-9]{19}$/
    }
  },
  methods: {
    zhuanchang_delete (zname, number, url, name) {
      var message = {zname: zname, number: number, url: url, name: name}
      var data = qs.stringify(message)
      this.$axios({method: 'post', url: 'product/guanli/zcdelete', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.status === 200) {
            this.$message.success('删除成功')
            // this.$router.go(0)
            // this.zhuanchang_get()
            this.zhuanchang_paimai_kaiqi(zname)
          } else {
            this.$message.error('删除失败')
          }
        })
    },
    zc_delete (zname, url) {
      var message = {zname: zname, url: url}
      var data = qs.stringify(message)
      this.$axios({method: 'post', url: 'product/shangjia/zc_delete', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.status === 200) {
            this.$message.success('删除成功')
            this.zhuanchang_get()
          } else {
            this.$message.error('删除失败')
          }
        })
    },
    del (name, number, url) {
      var message = {name: name, number: number, url: url}
      var data = qs.stringify(message)
      this.$axios({method: 'post', url: 'product/guanli/paimaidelete', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.status === 200) {
            this.$message.success('删除成功')
            this.paimaipin()
          } else {
            this.$message.error('删除失败')
          }
        })
    },
    current_change_zhuanchang (currentPage) {
      this.currentPage_zhuanchang = currentPage
    },
    shangjia_zhuanchang () {
      this.zhuanchang_form.zname = this.zhuanchang_zname
      var data = qs.stringify(this.zhuanchang_form)
      this.$axios({method: 'post', url: 'product/shangjia/shangjia_zhuanchang', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '500') {
            this.$message.error(response.data.message)
          } else {
            this.zcpm.splice(0, this.zcpm.length)
            this.zhuanchang_paimai_kaiqi(this.zhuanchang_zname)
            this.$message.success('添加成功')
            this.shangjiaquxiao_zhuanchang()
          }
        })
    },
    shangjiaquxiao_zhuanchang () {
      this.dialogFormVisible3 = false
      this.zhuanchang_form = {}
      this.onRemove_zhuanchang()
    },
    exceedUpload_zhuanchang () {
      this.$message.error('只能上传一个文件')
    },
    beforeAvatarUpload_zhuanchang (file) {
      const isJPG = file.type === 'image/jpeg' || file.type === 'image/gif' || file.type === 'image/png'
      const isLt2M = file.size / 1024 / 1024 < 5

      if (!isJPG) {
        this.$message.error('上传图片只能是 JPG/JPEG/PNG/GIF 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传图片大小不能超过5M!')
      }

      return isJPG && isLt2M
    },
    upload_zhuanchang (param) {
      var formData = new FormData()
      formData.append('file', param.file)
      this.$axios({method: 'post', data: formData, url: 'product/shangjia/zhuanchang_paimai_upload'})
        .then(response => {
          if (response.data.code === '200') {
            var filename = response.data.name[0]
            this.fileList_zhuanchang.push({name: filename, status: 'success'})
            this.$message.success('上传成功')
          } else if (response.data.code === '500') {
            this.$message.error(response.data.Message)
          }
        })
    },
    onRemove_zhuanchang () {
      this.$axios({method: 'post', url: 'product/shangjia/zhuanchang_paimai_xh'})
        .then(response => {
          if (response.data.code === '200') {
            this.fileList_zhuanchang.splice(0, 1)
          }
        })
    },
    paimai_zc_shangjia () {
      this.zhuanchang_form = {}
      this.onRemove_zhuanchang()
      this.dialogFormVisible3 = true
    },
    zhuanchang_paimai_kaiqi (zname) {
      this.zhuanchang_zname = zname
      var data = {zname: zname}
      data = qs.stringify(data)
      this.$axios({method: 'post', url: 'product/shangjia/getzcpm', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          this.zcpm.splice(0, this.zcpm.length)
          for (var i = 0; i < response.data.Table.length; i++) {
            this.zcpm.push(response.data.Table[i])
          }
          this.zcpm_length = this.zcpm.length
          for (var j = 0; j < this.zcpm.length; j++) {
            this.zcpm[j].starttime = this.zcpm[j].starttime.replace('T', ' ')
            this.zcpm[j].endtime = this.zcpm[j].endtime.replace('T', ' ')
            var starttime = this.zcpm[j].starttime
            var endtime = this.zcpm[j].endtime
            var time1 = starttime.split(' ')
            var time2 = time1[0].split('-')
            var time3 = time1[1].split(':')
            var date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            var date2 = date1.getTime()

            // 結束时间 date3
            time1 = endtime.split(' ')
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
              this.zcpm[j].state = '未开始'
            } else if (date4 > date3) {
              this.zcpm[j].state = '已结束'
            } else {
              this.zcpm[j].state = '拍卖中'
            }
          }
          this.cc2 = Math.ceil((this.zcpm_length / 6) * 10)
        })
      this.dialogFormVisible2 = true
    },
    upload_zc (param) {
      var formData = new FormData()
      formData.append('file', param.file)
      this.$axios({method: 'post', data: formData, url: 'product/shangjia/zhuanchang_upload'})
        .then(response => {
          if (response.data.code === '200') {
            var filename = response.data.name[0]
            this.fileList_zc.push({name: filename, status: 'success'})
            this.$message.success('上传成功')
          } else if (response.data.code === '500') {
            this.$message.error(response.data.Message)
          }
        })
    },
    beforeAvatarUpload_zc (file) {
      const isJPG = file.type === 'image/jpeg' || file.type === 'image/gif' || file.type === 'image/png'
      const isLt2M = file.size / 1024 / 1024 < 5

      if (!isJPG) {
        this.$message.error('上传图片只能是 JPG/JPEG/PNG/GIF 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传图片大小不能超过5M!')
      }

      return isJPG && isLt2M
    },
    exceedUpload_zc () {
      this.$message.error('只能上传一个文件')
    },
    onRemove_zc () {
      this.$axios({method: 'post', url: 'product/shangjia/zhuanchang_xh'})
        .then(response => {
          if (response.data.code === '200') {
            this.fileList_zc.splice(0, 1)
          }
        })
    },
    zhuanchangbutton () {
      this.zhuanchang = {}
      this.onRemove_zc()
      this.dialogFormVisible1 = true
    },
    zhuanchangquxiao () {
      this.zhuanchang = {}
      this.onRemove_zc()
      this.dialogFormVisible1 = false
    },
    zhuanchangsubmit () {
      var data = qs.stringify(this.zhuanchang)
      this.$axios({method: 'post', data: data, url: 'product/shangjia/zhuanchang_submit', headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '500') {
            this.$message.error(response.data.message)
          } else {
            this.zhuanchang_card.splice(0, this.zhuanchang_card.length)
            this.zhuanchang_get()
            this.$message.success('添加成功')
            this.zhuanchangquxiao()
          }
        })
    },
    current_change (currentPage) {
      this.currentPage = currentPage
    },
    current_change_zc (currentPage) {
      this.currentPage_zc = currentPage
    },
    link (name) {
      let routeUrl = this.$router.resolve({path: '/buy', query: {name: name, method: 'putong'}})
      window.open(routeUrl.href, '_blank')
    },
    linkz (name, zname) {
      let routeUrl = this.$router.resolve({path: '/buy', query: {name: name, zname: zname, method: 'zhuanchang'}})
      window.open(routeUrl.href, '_blank')
    },
    beforeAvatarUpload (file) {
      const isJPG = file.type === 'image/jpeg' || file.type === 'image/gif' || file.type === 'image/png'
      const isLt2M = file.size / 1024 / 1024 < 5

      if (!isJPG) {
        this.$message.error('上传图片只能是 JPG/JPEG/PNG/GIF 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传图片大小不能超过5M!')
      }

      return isJPG && isLt2M
    },
    exceedUpload () {
      this.$message.error('只能上传一个文件')
    },
    onRemove (file) {
      this.$axios({method: 'post', url: 'product/shangjia/paimai_xh'})
        .then(response => {
          if (response.data.code === '200') {
            this.fileList.splice(0, 1)
          }
        })
    },
    upload (param) {
      var formData = new FormData()
      formData.append('file', param.file)
      this.$axios({method: 'post', data: formData, url: 'product/shangjia/paimai_upload'})
        .then(response => {
          if (response.data.code === '200') {
            var filename = response.data.name[0]
            this.fileList.push({name: filename, status: 'success'})
            this.$message.success('上传成功')
          } else if (response.data.code === '500') {
            this.$message.error(response.data.Message)
          }
        })
    },
    paimai_shangjia () {
      this.onRemove()
      this.pai_mai.name = ''
      this.pai_mai.type = ''
      this.pai_mai.startprice = 0
      this.pai_mai.gapprice = 0
      this.pai_mai.time = []
      this.dialogFormVisible = true
    },
    shangjiaquxiao () {
      this.dialogFormVisible = false
      this.pai_mai.name = ''
      this.pai_mai.type = ''
      this.pai_mai.startprice = 0
      this.pai_mai.gapprice = 0
      this.pai_mai.time = []
      this.onRemove()
    },
    shangjia_paimai () {
      var data = qs.stringify(this.pai_mai)
      this.$axios({method: 'post', data: data, url: 'product/shangjia/paimai_submit', headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data.code === '500') {
            this.$message.error(response.data.message)
          } else {
            this.paimaipin()
            this.$message.success('添加成功')
            this.shangjiaquxiao()
          }
        })
    },
    tixian () {
      this.$refs['formt'].validate((valid) => {
        if (valid) {
          var data = qs.stringify(this.formt)
          this.$axios({method: 'post', data: data, url: 'product/shangjia/tixian', headers: {'content-type': 'application/x-www-form-urlencoded'}})
            .then(response => {
              if (response.data.code === '200') {
                this.getxinxi()
                this.$message.success('提现成功')
              } else {
                this.$message.error(response.data.message)
              }
            })
        }
      })
    },
    changexinxi () {
      this.$refs['formx'].validate((valid) => {
        if (valid) {
          var data = qs.stringify(this.formx)
          this.$axios({method: 'post', url: 'product/shangjia/changexinxi', data: data, headers: {'content-type': 'application/x-www-form-urlencoded'}})
            .then(response => {
              if (response.data.code === '200') {
                this.$message.success('修改成功')
                this.xinxi.gname = this.formx.gname
                this.xinxi.address = this.formx.address
                this.xinxi.phonenumber = this.formx.phonenumber
                this.changeid = true
              } else {
                this.$message.error('出错,请稍后再试!')
              }
            })
        }
      })
    },
    genggai () {
      this.formx.gname = this.xinxi.gname
      this.formx.address = this.xinxi.address
      this.formx.phonenumber = this.xinxi.phonenumber
      this.changeid = false
    },
    issj () {
      this.$axios({method: 'post', url: 'product/issj'})
        .then(response => {
          this.state = response.data.Table[0].state
          if (this.state !== 3) {
            this.$router.push({path: '/index'})
          }
        })
    },
    handleClick (tab) {
      if (tab.name === 'first') {
        this.isfirst = true
        this.issecond = false
        this.isthird = false
      } else if (tab.name === 'second') {
        this.isfirst = false
        this.issecond = true
        this.isthird = false
      } else if (tab.name === 'third') {
        this.isfirst = false
        this.issecond = false
        this.isthird = true
      }
    },
    handleSelect (key) {
      this.activeIndex = key
      if (key === '1') {
        this.formt.cardid = ''
        this.formt.money = ''
      } else {
        this.formx.gname = ''
        this.formx.address = ''
        this.formx.phonenumber = ''
      }
    },
    handleSelect1 (key) {
      this.activeIndex1 = key
    },
    getxinxi () {
      this.$axios({method: 'post', url: 'product/shangjia/getxinxi'})
        .then(response => {
          var data = response.data.Table[0]
          this.xinxi.gname = data.gname
          this.xinxi.address = data.address
          this.xinxi.phonenumber = data.phonenumber
          this.xinxi.money = data.money
        })
    },
    paimaipin () {
      this.$axios({method: 'post', url: 'product/shangjia/paimaipin'})
        .then(response => {
          var data = response.data.Table
          this.paimai_card_length = data.length
          this.paimai_card = []
          for (var i = 0; i < data.length; i++) {
            this.paimai_card.push(data[i])
          }
          for (var j = 0; j < this.paimai_card_length; j++) {
            this.paimai_card[j].starttime = this.paimai_card[j].starttime.replace('T', ' ')
            this.paimai_card[j].endtime = this.paimai_card[j].endtime.replace('T', ' ')
            // 现在的时间
            var date = new Date().getTime()
            var time1 = this.paimai_card[j].starttime.split(' ')
            var time2 = time1[0].split('-')
            var time3 = time1[1].split(':')
            var date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            // 开始拍卖的时间
            var date2 = date1.getTime()

            var time4 = this.paimai_card[j].endtime.split(' ')
            var time5 = time4[0].split('-')
            var time6 = time4[1].split(':')
            var date3 = new Date()
            date3.setFullYear(time5[0], time5[1] - 1, time5[2])
            date3.setHours(time6[0])
            date3.setMinutes(time6[1])
            date3.setSeconds(time6[2])
            // 结束拍卖的时间
            var date4 = date3.getTime()
            if (date < date2) {
              this.paimai_card[j].state = '未开始'
            } else if (date > date2 && date < date4) {
              this.paimai_card[j].state = '拍卖中'
            } else {
              if (this.paimai_card[j].cishu === 0) {
                this.paimai_card[j].state = '未成交'
              } else {
                this.paimai_card[j].state = '已成交, 成交人: ' + this.paimai_card[j].person + ',成交价: ' + this.paimai_card[j].nowprice
              }
            }
          }
          this.cc = Math.ceil((this.paimai_card_length / 6) * 10)
        })
    },
    jingpai () {
      this.$axios({method: 'post', url: 'product/shangjia/jingpai'})
        .then(response => {
          var data = response.data.Table
          for (var i = 0; i < data.length; i++) {
            data[i].time = data[i].time.replace('T', ' ')
            if (data[i].state === 0) {
              data[i].mbstate = '未成交'
            } else if (data[i].state === 1) {
              data[i].mbstate = '已成交'
            }
            this.jingpaicard.push(data[i])
          }
        })
    },
    zhuanchang_get () {
      this.$axios({method: 'post', url: 'product/shangjia/zhuanchang_get'})
        .then(response => {
          this.zhuanchang_card = []
          var data = response.data.Table
          this.zhuanchang_card_length = data.length
          for (var i = 0; i < data.length; i++) {
            this.zhuanchang_card.push(data[i])
          }
          for (var j = 0; j < this.zhuanchang_card.length; j++) {
            this.zhuanchang_card[j].starttime = this.zhuanchang_card[j].starttime.replace('T', ' ')
            this.zhuanchang_card[j].endtime = this.zhuanchang_card[j].endtime.replace('T', ' ')
            // 现在的时间
            var date = new Date().getTime()
            var time1 = this.zhuanchang_card[j].starttime.split(' ')
            var time2 = time1[0].split('-')
            var time3 = time1[1].split(':')
            var date1 = new Date()
            date1.setFullYear(time2[0], time2[1] - 1, time2[2])
            date1.setHours(time3[0])
            date1.setMinutes(time3[1])
            date1.setSeconds(time3[2])
            // 开始拍卖的时间
            var date2 = date1.getTime()

            var time4 = this.zhuanchang_card[j].endtime.split(' ')
            var time5 = time4[0].split('-')
            var time6 = time4[1].split(':')
            var date3 = new Date()
            date3.setFullYear(time5[0], time5[1] - 1, time5[2])
            date3.setHours(time6[0])
            date3.setMinutes(time6[1])
            date3.setSeconds(time6[2])
            // 结束拍卖的时间
            var date4 = date3.getTime()
            if (date < date2) {
              this.zhuanchang_card[j].state = '专场未开始'
            } else if (date > date2 && date < date4) {
              this.zhuanchang_card[j].state = '专场进行中'
            } else {
              this.zhuanchang_card[j].state = '专场已结束'
            }
          }
          this.cc1 = Math.ceil((this.zhuanchang_card_length / 12) * 10)
        })
    }
  },
  mounted: function () {
    this.issj()
    this.getxinxi()
    this.paimaipin()
    this.jingpai()
    this.zhuanchang_get()
  }
}
</script>
<style scoped>
  .image {
    width: 100%;
    height: 300px;
    display: block;
  }
  .categories {
    position: fixed;
    margin-left: 50%;
    left: -600px;
    top: 100px;
    width: 150px;
  }
  .bottom {
    margin-top: 13px;
    line-height: 12px;
  }
  .clearfix:before,
  .clearfix:after {
      display: table;
      content: "";
  }
  .clearfix:after {
      clear: both
  }
  .time {
    font-size: 13px;
    color: #999;
  }
  .button {
    padding: 0;
    float: right;
  }
</style>
