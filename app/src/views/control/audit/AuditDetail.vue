<template>
  <a-modal
    title="日志详情"
    :width="800"
    :visible="visible"
    :confirmLoading="confirmLoading"
    :footer="null"
    @ok="handleOk"
    @cancel="handleCancel"
  >
    <a-card :bordered="false">
      <detail-list title>
        <detail-list-item term="操作名称" style="width:50%">{{ detailData.subject }}</detail-list-item>
        <detail-list-item term="日志类型" style="width:50%">{{ detailData.logType | loggerType }}</detail-list-item>
        <detail-list-item term="操作账户" style="width:50%">
          <span>{{ detailData.accountName }}</span>
          <a-tag color="blue">{{ "ID:"+detailData.accountId }}</a-tag>
        </detail-list-item>
        <detail-list-item term="客户端IP" style="width:50%">{{ detailData.clientIp }}</detail-list-item>
        <detail-list-item term="权限标识" style="width:50%">{{ detailData.permissionId }}</detail-list-item>
        <detail-list-item
          term="记录时间"
          style="width:50%"
        >{{ detailData.addTime | dayFormat('YYYY-MM-DD HH:mm:ss') }}</detail-list-item>
        <detail-list-item
          term="备注"
          style="width:100%"
        >{{ (detailData.remark===null ? '--':detailData.remark) }}</detail-list-item>
      </detail-list>
      <a-divider style="margin-bottom: 32px"/>
      <detail-list title>
        <detail-list-item
          term="操作API"
          style="width:100%"
        >{{ (detailData.apiAddress===null ? '--':detailData.apiAddress) }}</detail-list-item>
      </detail-list>
      <detail-list title>
        <detail-list-item term="请求数据" style="width:100%">
          {{ (hidden ? '--':'') }}
          <a-row>
            <p v-if="isStr">{{ jsonArray }}</p>
            <JsonView v-else :hidden="hidden" :json="jsonSource" :closed="true"></JsonView>
          </a-row>
        </detail-list-item>
      </detail-list>
      <detail-list title>
        <detail-list-item term="响应数据" style="width:100%">
          {{ (hiddenResp ? '--':'') }}
          <a-row>
            <p v-if="isStrResp">{{ jsonArrayResp }}</p>
            <JsonView v-else :hidden="hiddenResp" :json="jsonSourceResp" :closed="true"></JsonView>
          </a-row>
        </detail-list-item>
      </detail-list>
    </a-card>
  </a-modal>
</template>

<script>
import JsonView from '@/components/JsonView/JsonView'
import DetailList from '@/components/tools/DetailList'
const DetailListItem = DetailList.Item

export default {
  name: 'AuditDetail',
  components: {
    DetailList,
    DetailListItem,
    JsonView
  },
  data () {
    return {
      labelCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 5
        }
      },
      wrapperCol: {
        xs: {
          span: 24
        },
        sm: {
          span: 16
        }
      },

      visible: false, // 弹框是否显示
      confirmLoading: false, // 弹框中的提交按钮是否加载中
      detailData: {}, // 对象
      jsonSource: [],
      isStr: false,
      jsonArray: '',

      jsonSourceResp: [],
      hiddenResp: false,
      jsonArrayResp: '',
      isStrResp: false,
      hidden: false
    }
  },
  filters: {
    loggerType (status) {
      const statusMap = {
        1: '登陆日志',
        2: '操作日志'
      }
      return statusMap[status === 1 ? 1 : 2]
    }
  },
  created () {},
  methods: {
    showModal (record) {
      this.visible = true
      this.detailData = record
      console.log(this.detailData)
      if (this.detailData.apiRequest !== null && !this.detailData.apiAddress !== null) {
        this.makeJsonDataReq(this.detailData.apiRequest)
      } else {
        this.hidden = true
      }
      if (this.detailData.apiResponse !== null) {
        this.makeJsonDataResp(this.detailData.apiResponse)
      } else {
        this.hiddenResp = true
      }
    },
    // 关闭方法
    close () {
      this.detailData = {}
      this.jsonSource = []
      this.jsonArray = ''
      this.jsonSourceResp = []
      this.hiddenResp = false
      this.jsonArrayResp = ''
      this.hidden = false

      this.$emit('close')
      this.visible = false
    },
    // 弹框提交方法
    handleOk () {},
    handleCancel () {
      this.close()
    },
    // 构建响应json数据
    makeJsonDataResp (value) {
      try {
        if (this.isJson(value)) {
          const data = JSON.parse(value)
          this.jsonSourceResp = data
          this.isStrResp = false
        } else {
          this.jsonSourceResp = value
          this.jsonArrayResp = value
          this.isStrResp = true
        }
      } catch (err) {
        this.jsonArrayResp = value
        console.error(err)
      }
    },
    // 构建请求json数据
    makeJsonDataReq (value) {
      try {
        if (this.isJson(value)) {
          const data = JSON.parse(value)
          this.jsonSource = data
          this.isStr = false
        } else {
          this.jsonSource = value
          this.jsonArray = value
          this.isStr = true
        }
      } catch (err) {
        this.jsonArray = value
        console.error(err)
      }
    },
    isJson (value) {
      if (typeof value === 'string') {
        try {
          var obj = JSON.parse(value)
          if (typeof obj === 'object' && obj) {
            return true
          } else {
            return false
          }
        } catch (e) {
          return false
        }
      }
    }
  }
}
</script>

<style scoped>
.row {
  padding: 10px;
}
.col-text-right {
  text-align: right;
}
/* .col-backgroud-color1{
               background: rgba(0, 160, 233, 0.7);
            }
            .col-backgroud-color2{
               background: #00a0e9;
            } */
</style>
