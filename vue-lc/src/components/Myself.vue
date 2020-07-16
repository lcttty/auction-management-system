<template>
  <div>
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
            <span slot="title">个人信息</span>
          </el-menu-item>
          <el-menu-item index="2">
            <i class="el-icon-menu"></i>
            <span slot="title">密码修改</span>
          </el-menu-item>
          <el-menu-item index="3">
            <i class="el-icon-menu"></i>
            <span slot="title">充值服务</span>
          </el-menu-item>
          <el-menu-item index="4">
            <i class="el-icon-menu"></i>
            <span slot="title">购买记录</span>
          </el-menu-item>
          <div v-if="sj != 3">
          <el-menu-item index="5">
            <i class="el-icon-menu"></i>
            <span slot="title">成为商家</span>
          </el-menu-item>
          </div>
          <div v-if="sj == 3">
            <el-menu-item index="5"  @click="tosj">
            <i class="el-icon-menu"></i>
            <span slot="title">商家控制台</span>
            </el-menu-item>
          </div>
        </el-menu>
      </el-col>
      <el-col :span="20" style="margin-left:16.7%;margin-top:40px;">
        <div v-if="a1" style="border:1px solid #e3e3e3;border-radius:0px;height:150px;">
          <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">您的余额:<span style="margin-left:30px;color:red">{{ myself.money }}</span></div>
          <div v-if="change">
            <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">您的昵称:<span style="margin-left:30px;color:red">{{ myself.name }}</span>
              <el-button type="primary" style="margin-left:10%;width: 20%; background: #505458;border: none" @click="change=false">更改</el-button>
            </div>
          </div>
          <div v-else>
            <div style="font-size:20px;margin-top:30px;text-align:left;margin-left:30px;">
                您的昵称:
                <el-input type="text" v-model="changeid" style="width: 20%;"></el-input>
                <el-button type="primary" style="margin-left:5%;width: 20%; background: #505458;border: none" @click="changeii">更改</el-button>
                <el-button type="primary" style="margin-left:5%;width: 20%; background: #505458;border: none" @click="change=true">取消</el-button>
            </div>
          </div>
        </div>
        <div v-if="a2" style="margin-top:50px;margin-left:30px;">
          <el-form label-width="80px" :model="form1" status-icon :rules="rules" ref="form1">
            <el-form-item label="原密码" style="width:50%;" prop="oldpass">
              <el-input type="text" v-model="form1.oldpass"></el-input>
            </el-form-item>
            <el-form-item label="新密码" style="width:50%;" prop="newpass">
              <el-input type="text" v-model="form1.newpass"></el-input>
            </el-form-item>
            <el-form-item style="width:50%;">
              <el-button  type="primary" style="width: 100%; background: #505458;border: none" @click="submitForm('form1')">更改密码</el-button>
            </el-form-item>
          </el-form>
        </div>
        <div v-if="a3" style="margin-top:50px;margin-left:30px;">
          <el-form label-width="80px" :model="form2" status-icon ref="form2" :rules="rules2">
            <el-form-item label="充值码" style="width:50%;" prop="xx">
              <el-input type="text" v-model="form2.xx" placeholder="请输入十位充值码"></el-input>
            </el-form-item>
            <el-form-item style="width:50%;">
              <el-button type="primary" style="width: 100%; background: #505458;border: none" @click="submitForm2('form2')">充值</el-button>
            </el-form-item>
          </el-form>
        </div>
        <div v-if="a4">
          <el-table :data="tableData" style="width: 100%;">
            <el-table-column prop="name" label="物品名" width="180">
            </el-table-column>
            <el-table-column prop="price" label="物品价格" width="180">
            </el-table-column>
            <el-table-column prop="time" label="时间" width="180">
            </el-table-column>
            <el-table-column prop="state" label="成交状态" width="180">
            </el-table-column>
          </el-table>
        </div>
        <div v-if="a5 && sj==1" style="margin-top:50px;margin-left:30px;">
          <el-row>
          <el-col :span="12">
          <el-form label-width="80px" :model="form3" status-icon :rules="rules3" ref="form3">
            <el-form-item label="公司名称" style="width: 70%;" prop="gname">
              <el-input type="text" v-model="form3.gname" placeholder="请输入公司名称"></el-input>
            </el-form-item>
            <el-form-item label="公司地址" style="width: 70%;" prop="address">
              <el-input type="text" v-model="form3.address" placeholder="请输入公司地址"></el-input>
            </el-form-item>
            <el-form-item label="电话号码" style="width: 70%" prop="phonenumber">
              <el-input type="text" v-model="form3.phonenumber" placeholder="请输入电话号码"></el-input>
            </el-form-item>
            <el-button type="primary" style="width: 100%; background: #505458;border: none" @click="allsubmit('form3')">确认提交</el-button>
          </el-form>
          </el-col>
          <el-col :span="12" style="text-align:left">
          <el-upload
              class="avatar-uploader"
              action="http://localhost:55587/api/product/cwsj-upload"
              :http-request="upload"
              :file-list="fileList"
              list-type="picture"
              :on-remove="onRemove"
              :with-credentials="true"
              :before-upload="beforeAvatarUpload"
              :limit="5"
              :on-exceed="exceedUpload">
              <el-button size="small" type="primary">点击上传</el-button>
              <div slot="tip" class="el-upload_tip">上传公司凭证、流水等</div>
              <div slot="tip" class="el-upload_tip">只能上传jpg/png文件,且不超过5mb</div>
          </el-upload>
          </el-col>
          </el-row>
        </div>
        <div v-if="a5 && sj == 2">
          正在审核中...
        </div>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import qs from 'qs'
export default {
  name: 'Myself',
  data () {
    var validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入原密码'))
      } else if (!this.pattern.test(value)) {
        callback(new Error('原密码格式不正确'))
      } else {
        callback()
      }
    }
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入要更改的密码'))
      } else if (!this.pattern.test(value)) {
        callback(new Error('密码格式不正确'))
      } else {
        callback()
      }
    }
    var validateXx = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入充值码'))
      } else if (!this.pattern2.test(value)) {
        callback(new Error('充值码格式不正确'))
      } else {
        callback()
      }
    }
    var validateGname = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入公司名称'))
      } else if (!this.pattern3.test(value)) {
        callback(new Error('名称长度过长'))
      } else {
        callback()
      }
    }
    var validateAddress = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入公司地址'))
      } else if (!this.pattern3.test(value)) {
        callback(new Error('名称长度过长'))
      } else {
        callback()
      }
    }
    var validatePhonenumber = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入电话号码'))
      } else if (!this.pattern4.test(value)) {
        callback(new Error('电话号码格式不正确'))
      } else {
        callback()
      }
    }
    return {
      sj: 1,
      myself: {},
      fileList: [],
      imageUrl: '',
      activeIndex: '1',
      a1: false,
      a2: false,
      a3: false,
      a4: false,
      a5: false,
      form1: {
        oldpass: '',
        newpass: ''
      },
      form2: {
        xx: ''
      },
      form3: {
        gname: '',
        address: '',
        phonenumber: ''
      },
      rules: {
        oldpass: [
          { validator: validatePass, trigger: 'blur' }
        ],
        newpass: [
          { validator: validatePass2, trigger: 'blur' }
        ]
      },
      rules2: {
        xx: [
          {validator: validateXx, trigger: 'blur'}
        ]
      },
      rules3: {
        gname: [
          {validator: validateGname, trigger: 'blur'}
        ],
        address: [
          {validator: validateAddress, trigger: 'blur'}
        ],
        phonenumber: [
          {validator: validatePhonenumber, trigger: 'blur'}
        ]
      },
      pattern: /^[a-zA-Z0-9]{6,18}$/,
      pattern2: /^[a-zA-Z0-9]{10}$/,
      pattern3: /^.{0,30}$/,
      pattern4: /^[0-9]{11}$/,
      tableData: [],
      change: true,
      changeid: ''
    }
  },
  methods: {
    tosj () {
      this.$router.push({path: '/shangjia'})
    },
    issj () {
      this.$axios({method: 'post', url: 'product/issj'})
        .then(response => {
          var data = response.data
          var state = data.Table[0].state
          this.sj = state
        })
    },
    allsubmit (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          this.$axios({method: 'post', url: 'product/checkupload'})
            .then(response => {
              if (response.data.code === '500') {
                this.$message.error('请先上传文件')
              } else {
                var formData = {gname: this.form3.gname, address: this.form3.address, phonenumber: this.form3.phonenumber}
                this.$axios({method: 'post', url: 'product/cwsj_allsubmit', data: qs.stringify(formData), headers: {'content-type': 'application/x-www-form-urlencoded'}})
                  .then(response => {
                    if (response.data.code === '200') {
                      this.form3.gname = ''
                      this.form3.address = ''
                      this.form3.phonenumber = ''
                      this.sj = 2
                      this.$message.success(response.data.message)
                    } else {
                      this.$message.error(response.data.message)
                    }
                  })
              }
            })
        } else {
          this.$message.error('格式不正确')
        }
      })
    },
    exceedUpload (files, fileList) {
      this.$message.error('最多上传五个文件!')
    },
    onRemove (file) {
      var fileList = this.fileList
      var name = ''
      for (var i = 0; i < fileList.length; i++) {
        if (fileList[i].name === file.name) {
          fileList.splice(i, 1)
          name = file.name
        }
      }
      var date = {name: name}
      this.$axios({method: 'post', data: qs.stringify(date), url: 'product/cwsj-upload-xh', headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
        })
    },
    upload (param) {
      var formData = new FormData()
      formData.append('file', param.file)
      this.$axios({method: 'post', data: formData, url: 'product/cwsj-upload'})
        .then(response => {
          var data = response.data
          if (data.code === '200') {
            this.fileList.push({name: data.name[0], status: 'success', url: data.url})
          } else {
            this.$message.error(data.Message)
          }
        })
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
    changeii () {
      var date = {id: this.changeid}
      this.$axios({method: 'post', data: qs.stringify(date), url: 'product/changeid', headers: {'content-type': 'application/x-www-form-urlencoded'}})
        .then(response => {
          if (response.data === 'state1') {
            this.$router.push({path: '/login'})
          } else if (response.data === 'state3') {
            this.myself1()
            this.change = true
            this.$message({message: '昵称修改成功', type: 'success'})
          } else if (response.data === 'state2') {
            this.$message({message: '昵称格式不正确', type: 'error'})
          }
        })
    },
    myself1 () {
      this.$axios({method: 'post', url: 'product/myself'})
        .then(response => {
          if (response.data === 'state1') {
            this.$router.push({path: '/login'})
          } else {
            this.myself = response.data.Table[0]
          }
        })
    },
    myself2 () {
      this.$axios({method: 'post', url: 'product/myself1'})
        .then(response => {
          if (response.data === 'state1') {
            this.$router.push({path: '/login'})
          } else if (response.data === 'state2') {
          } else {
            var data = response.data.Table
            for (var i in data) {
              if (data[i].name.length > 10) {
                data[i].name = data[i].name.substr(0, 10) + '...'
              }
              data[i].time = data[i].time.replace('T', ' ')
              if (data[i].state === 0) {
                data[i].state = '未成交'
              } else if (data[i].state === 1) {
                data[i].state = '已成交'
              }
              this.tableData.push(data[i])
            }
          }
        })
    },
    handleSelect (key, keyPath) {
      if (key === '1') {
        this.a1 = true
        this.a2 = false
        this.a3 = false
        this.a4 = false
        this.a5 = false
        this.form1.oldpass = ''
        this.form1.newpass = ''
        this.form2.xx = ''
        this.form3.gname = ''
        this.form3.address = ''
        this.form3.phonenumber = ''
      }
      if (key === '2') {
        this.a1 = false
        this.a2 = true
        this.a3 = false
        this.a4 = false
        this.a5 = false
        this.form2.xx = ''
        this.form3.gname = ''
        this.form3.address = ''
        this.form3.phonenumber = ''
      }
      if (key === '3') {
        this.a1 = false
        this.a2 = false
        this.a3 = true
        this.a4 = false
        this.a5 = false
        this.form1.oldpass = ''
        this.form1.newpass = ''
        this.form3.gname = ''
        this.form3.address = ''
        this.form3.phonenumber = ''
      }
      if (key === '4') {
        this.a1 = false
        this.a2 = false
        this.a3 = false
        this.a4 = true
        this.a5 = false
        this.form1.oldpass = ''
        this.form1.newpass = ''
        this.form2.xx = ''
        this.form3.gname = ''
        this.form3.address = ''
        this.form3.phonenumber = ''
      }
      if (key === '5') {
        this.a1 = false
        this.a2 = false
        this.a3 = false
        this.a4 = false
        this.a5 = true
        this.form1.oldpass = ''
        this.form1.newpass = ''
        this.form2.xx = ''
      }
    },
    submitForm (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          var oldpass = this.form1.oldpass
          var newpass = this.form1.newpass
          oldpass = encodeURIComponent(encodeURIComponent(this.$getCode(oldpass)))
          newpass = encodeURIComponent(encodeURIComponent(this.$getCode(newpass)))
          this.$axios({method: 'post', url: 'product/gaimi/' + oldpass + '/' + newpass})
            .then(response => {
              if (response.data === 'error1' || response.data === 'error2') {
                this.$message({message: '对不起,您还未登录,即将为您跳转至登录页面!', type: 'error'})
                setTimeout(() => {
                  this.$router.push({path: '/login'})
                }, 2000)
              } else if (response.data === 'error3') {
                this.$message({message: '旧密码不正确!', type: 'error'})
              } else if (response.data === 'error4') {
                this.$message({message: '服务器出现错误,请稍后再试!', type: 'error'})
              } else if (response.data === 'ok') {
                this.$message.success('密码更改成功!')
              } else {
                this.$message.error(response.data)
              }
              this.form1.oldpass = ''
              this.form1.newpass = ''
            })
        } else {
          this.$message({message: '密码格式不正确', type: 'error'})
          this.form1.oldpass = ''
          this.form1.newpass = ''
        }
      })
    },
    submitForm2 (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          // this.form2.xx = ''
          var xx = this.form2.xx
          xx = encodeURIComponent(encodeURIComponent(this.$getCode(xx)))
          this.$axios({method: 'post', url: 'product/chongzhi/' + xx})
            .then(response => {
              if (response.data === 'ok') {
                this.form2.xx = ''
                this.$message.success('充值成功')
              } else if (response.data === 'state1') {
                this.$message.error('充值码不正确')
              } else if (response.data === 'state2') {
                this.$message.error('未登录,即将为您跳转')
                setTimeout(() => {
                  this.$router.push({path: '/login'})
                }, 2000)
              } else if (response.data === 'state3') {
                this.$message.error('数据库异常')
              } else {
                this.$message.error('服务器异常')
                console.log(response.data)
              }
            })
        } else {
        }
      })
    }
  },
  mounted () {
    this.issj()
    this.myself1()
    this.myself2()
    this.a1 = true
    this.$axios({method: 'post', url: 'product/cwsj_destory'})
  },
  beforeDestroy () {
    this.$axios({method: 'post', url: 'product/cwsj_destory'})
  }
}
</script>
<style scoped>
  .categories {
    position: fixed;
    margin-left: 50%;
    left: -600px;
    top: 100px;
    width: 150px;
  }
   .avatar-uploader .el-upload {
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .avatar-uploader .el-upload:hover {
    border-color: #409EFF;
  }
  .avatar-uploader-icon {
    font-size: 28px;
    color: #8c939d;
    width: 178px;
    height: 178px;
    line-height: 178px;
    text-align: center;
  }
  .avatar {
    width: 178px;
    height: 178px;
    display: block;
  }
</style>
